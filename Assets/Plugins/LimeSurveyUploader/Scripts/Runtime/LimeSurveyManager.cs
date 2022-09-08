using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using GameLabGraz.LimeSurvey.Data;
using GameLabGraz.LimeSurvey.Extensions;
using GEAR.Gadgets.Coroutine;
using Newtonsoft.Json.Linq;
using UnityEngine.Events;

namespace GameLabGraz.LimeSurvey
{
    public class LimeSurveyManager : MonoBehaviour
    {
        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Members

        // -------------------------------------------------------------------------------------------------------------
        // Server Configuration
        private string url;
        private string userName;
        private string password;

        // -------------------------------------------------------------------------------------------------------------
        // Survey Configuration
        [SerializeField] private string surveyId;

        // -------------------------------------------------------------------------------------------------------------
        // Other
        private JsonRpcClient _client;

        public string SessionKey { get; private set; }

        public bool LoggedIn { get; private set; }

        private static LimeSurveyManager _instance;
        
        [HideInInspector] public ErrorEvent OnError;
        [HideInInspector] public WarningEvent OnWarning;
        [HideInInspector] public UnityEvent OnStartLogin;
        [HideInInspector] public LoginEvent OnLoggedIn;

        private string _lastError = "";

        // +++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Methods

        public static LimeSurveyManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<LimeSurveyManager>();
                return _instance;
            }
        }

        private void Awake()
        {
            ReadServerConfigFile();
            _client = new JsonRpcClient(url);
            StartCoroutine(Login());
        }

        private void ReadServerConfigFile()
        {
            // Read file
            TextAsset jsonFile = Resources.Load("LimeSurveyServerConfig/LimeSurveyServerConfig") as TextAsset;

            // Decode JSON
            JObject jsonObject;
            try
            {
                jsonObject = JObject.Parse(jsonFile.text);
            }
            catch(System.Exception)
            {
                _lastError = "LimeSurveyServerConfig.json not found or format is invalid.";
                Debug.LogError("[LimeSurvey] " + _lastError); 
                OnError.Invoke("Cannot load Lime Survey settings.", _lastError);
                throw;
            }
            
            // Apply values
            this.url        = (string)jsonObject["url"];
            this.userName   = (string)jsonObject["username"];
            this.password   = (string)jsonObject["password"];
        }

        private IEnumerator Login()
        {
            OnStartLogin.Invoke();
            
            _client.ClearParameters();
            _client.SetMethod(LimeSurveyMethod.GetSessionKey);
            _client.AddParameter(LimeSurveyParameter.UserName, userName);
            _client.AddParameter(LimeSurveyParameter.Password, password);

            yield return _client.Post();

            if (HandleClientResponse(_client.Response) != ErrorCode.OK)
            {
                OnError.Invoke("Unable to login.", _lastError);
                yield return null;
            }

            var response = _client.Response.Result.ToString();
            if (response.Contains("\"status\""))
            {
                _lastError = "Unable to login: Invalid user name or password.";
                Debug.Log("[LimeSurvey] ERROR: Login: " + _lastError);
                OnError.Invoke("Unable to login.", "Invalid user name or password.");
            }
            else
            {
                _lastError = "";
                SessionKey = response;
                LoggedIn = true;
                Debug.Log($"[LimeSurvey] Login: Login successfully - Session Key: {SessionKey}");
                OnLoggedIn.Invoke(SessionKey);
            }
        }

        private ErrorCode HandleClientResponse(JsonRpcResponse response)
        {
            if (response.StatusCode != 200)
            {
                _lastError = $"Error - HTTP Status Code: {_client.Response.StatusCode}";
                Debug.Log("[LimeSurvey] ERROR: HandleClientResponse: " + _lastError);
                return ErrorCode.HttpStatusError;
            }
            if (_client.Response.Error != null)
            {
                _lastError = $"Error: {_client.Response.Error}";
                Debug.Log("[LimeSurvey] ERROR: HandleClientResponse: " + _lastError);
                return ErrorCode.ResponseError;
            }

            _lastError = "";
            return ErrorCode.OK;
        }

        private IEnumerator SetQuestionProperties(Question question)
        {
            _client.ClearParameters();
            _client.SetMethod(LimeSurveyMethod.GetQuestionProperties);
            _client.AddParameter(LimeSurveyParameter.SessionKey, SessionKey);
            _client.AddParameter(LimeSurveyParameter.QuestionID, question.ID);

            yield return _client.Post();

            if (HandleClientResponse(_client.Response) != ErrorCode.OK)
                yield return null;

            var questionProperties = (JObject)_client.Response.Result;
            // Attributes
            var attributes = questionProperties["attributes"];
            if (attributes != null)
            {
                var randOrder = attributes["random_order"];
                question.RandomOrder = randOrder != null && randOrder.ToString() == "1";
            }
            
            // SubQuestions
            var subQuestions = questionProperties["subquestions"];
            if (subQuestions != null && subQuestions.ToString() != "No available answers")
            {
                foreach (var subQuestion in subQuestions)
                {
                    var subQuestionObj = JsonUtility.FromJson<SubQuestion>(subQuestion.First?.ToString());
                    subQuestionObj.SubquestionName = (subQuestion as JProperty)?.Name;
                    question.SubQuestions.Add(subQuestionObj);
                }

                question.SubQuestions.Sort((lhs, rhs) => String.Compare(lhs.SubquestionName, rhs.SubquestionName, StringComparison.Ordinal));
                
                if (question.Other)
                {
                    var otherSubQuestion = JsonUtility.FromJson<SubQuestion>("{\"title\": \"other\", \"question\": \"Other:\"}");
                    question.SubQuestions.Add(otherSubQuestion);
                }
            }

            // AnswerOptions
            var answerOptions = questionProperties["answeroptions"];
            if (answerOptions != null && answerOptions.ToString() != "No available answer options")
            {
                foreach (var answerOption in answerOptions)
                {
                    string answerCodeJson;
                    var answerCode = answerOption.Path.Split('.').Last();

                    if (answerCode.StartsWith("answeroptions"))
                    {
                        answerCodeJson = $"{{\"answer_code\": \"{answerOption["order"]}\"," + answerOption.ToString().Remove(0,1);
                    }
                    else
                    {
                        answerCodeJson =$"{{\"answer_code\": \"{answerCode}\"," + 
                                        answerOption.First?.ToString().Remove(0, 1);
                    }

                    var answerOptionObj = JsonUtility.FromJson<AnswerOption>(answerCodeJson);
                    question.AnswerOptions.Add(answerOptionObj);
                }
            }
            switch (question.QuestionType)
            {
                case QuestionType.FivePointChoice:
                case QuestionType.FivePointMatrix:
                    SetAnswerOptionsForPointQuestion(question, 5);
                    break;
                case QuestionType.TenPointMatrix:
                    SetAnswerOptionsForPointQuestion(question, 10);
                    break;
                case QuestionType.Matrix:
                case QuestionType.ListRadio:
                    AddNoAnswerOption(question);
                    break;
            }
        }

        private static void AddNoAnswerOption(Question question)
        {
            if (question.Mandatory) return;
            question.AnswerOptions.Add(new AnswerOption("NA", "No answer", question.AnswerOptions.Count));
        }

        private static void SetAnswerOptionsForPointQuestion(Question question, int optionSize)
        {
            for (var point = 1; point <= optionSize; point++)
                question.AnswerOptions.Add(new AnswerOption($"{point}", $"{point}", point-1));

            AddNoAnswerOption(question);
        }

        public IEnumerator GetQuestionGroups()
        {
            while (!LoggedIn) yield return new WaitForSeconds(0.1f);

            _client.ClearParameters();
            _client.SetMethod(LimeSurveyMethod.ListGroups);
            _client.AddParameter(LimeSurveyParameter.SessionKey, SessionKey);
            _client.AddParameter(LimeSurveyParameter.SurveyID, surveyId);

            yield return _client.Post();

            var groupList = new List<QuestionGroup>();
            foreach(var group in (JArray)_client.Response.Result)
            {
                var groupObj = JsonUtility.FromJson<QuestionGroup>(group.ToString());

                var cd = new CoroutineWithData(this, GetQuestionList(groupObj.ID));
                yield return cd.Coroutine;
                
                groupObj.Questions.AddRange((List<Question>)cd.Result);
                groupList.Add(groupObj);
            }
            yield return groupList.OrderBy(g => g.GroupOrder).ToList();
        }

        public IEnumerator GetQuestionList(int? groupId = null)
        {
            while (!LoggedIn) yield return new WaitForSeconds(0.1f);

            _client.ClearParameters();
            _client.SetMethod(LimeSurveyMethod.ListQuestions);
            _client.AddParameter(LimeSurveyParameter.SessionKey, SessionKey);
            _client.AddParameter(LimeSurveyParameter.SurveyID, surveyId);
            if (groupId != null) 
                _client.AddParameter(LimeSurveyParameter.GroupID, groupId);

            yield return _client.Post();

            if (HandleClientResponse(_client.Response) != ErrorCode.OK)
                yield return null;

            var questionList = new List<Question>();
            foreach (var question in (JArray)_client.Response.Result)
            {
                var questionObj = JsonUtility.FromJson<Question>(question.ToString());
                if(questionObj.ParentID != 0) continue;

                // questionObj.Randomized = question["title"].ToString().Contains("randomized");

                yield return StartCoroutine(SetQuestionProperties(questionObj));
                
                // questionObj.SubQuestions.Reverse();
                if(questionObj.RandomOrder)
                    questionObj.SubQuestions = questionObj.SubQuestions.OrderBy(a => Guid.NewGuid()).ToList();


                questionList.Add(questionObj);
            }
            yield return questionList.OrderBy(q => q.QuestionOrder).ToList();
        }

        public IEnumerator UploadQuestionResponses(IEnumerable<Question> questions, int responseID = -1)
        {
            var responseData = new JObject();
            if (responseID != -1)
                responseData.Add("id", responseID);

            foreach (var question in questions)
            {
                if (question.SubQuestions.Count > 0)
                    foreach (var subQuestion in question.SubQuestions)
                        responseData.Add($"{surveyId}X{question.GID}X{question.ID}{subQuestion.Title}", subQuestion.Answer);
                else
                    responseData.Add($"{surveyId}X{question.GID}X{question.ID}", question.Answer);

            }

            _client.ClearParameters();
            _client.SetMethod(LimeSurveyMethod.AddResponse);
            _client.AddParameter(LimeSurveyParameter.SessionKey, SessionKey);
            _client.AddParameter(LimeSurveyParameter.SurveyID, surveyId);
            _client.AddParameter(LimeSurveyParameter.ResponseData, responseData);
            
            yield return _client.Post();

            if (HandleClientResponse(_client.Response) != ErrorCode.OK)
            {
                Debug.Log("[LimeSurvey] ERROR UploadQuestionResponses: Unable to upload responses.");
                OnError.Invoke("Unable to upload responses", _lastError);
                yield return -1;
            }

            yield return int.Parse(_client.Response.Result.ToString());
        }

        public string GetLastError()
        {
            return _lastError;
        }
    }
}
