using System;
using UnityEngine;
using System.Net;
using System.Collections.Generic;
using GameLabGraz.LimeSurvey.Data;
using GameLabGraz.LimeSurvey.Extensions;
using Unity.Plastic.Newtonsoft.Json.Linq;

namespace GameLabGraz.LimeSurvey
{
    public class LimeSurveyManager : MonoBehaviour
    {
        [Header("Login")]
        [SerializeField] private string url;
        [SerializeField] private string userName;
        [SerializeField] private string password;
        [SerializeField] private string surveyId;

        private JsonRpcClient _client;

        public string SessionKey { get; private set; }


        private static LimeSurveyManager _instance;

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
            _client = new JsonRpcClient(url);
            Login();
        }

        private void Login()
        {
            _client.ClearParameters();
            _client.SetMethod(LimeSurveyMethod.GetSessionKey);
            _client.AddParameter(LimeSurveyParameter.UserName, userName);
            _client.AddParameter(LimeSurveyParameter.Password, password);
            _client.Post();

            if (HandleClientResponse(_client.Response) != ErrorCode.OK)
                return;

            var response = _client.Response.result.ToString();
            if (response.Contains("\"status\""))
            {
                Debug.LogError("LimeSurveyManager::Login: Invalid user name or password.");
            }
            else
            {
                SessionKey = response;
                Debug.Log($"LimeSurveyManager::Login: Login successfully - Session Key: {SessionKey}");
            }
        }

        private ErrorCode HandleClientResponse(JsonRpcResponse response)
        {
            if (_client.Response.StatusCode != HttpStatusCode.OK)
            {
                Debug.LogError($"LimeSurveyManager::HandleClientResponse: Error - HTTP Status Code: {_client.Response.StatusCode}");
                return ErrorCode.HttpStatusError;
            }
            else if (_client.Response.error != null)
            {
                Debug.LogError($"LimeSurveyManager::HandleClientResponse: Error: {_client.Response.error}");
                return ErrorCode.ResponseError;
            }
            return ErrorCode.OK;
        }

        private void SetQuestionProperties(Question question)
        {
            _client.ClearParameters();
            _client.SetMethod(LimeSurveyMethod.GetQuestionProperties);
            _client.AddParameter(LimeSurveyParameter.SessionKey, SessionKey);
            _client.AddParameter(LimeSurveyParameter.QuestionID, question.ID);
            _client.Post();

            if (HandleClientResponse(_client.Response) != ErrorCode.OK)
                return;

            var questionProperties = (JObject)_client.Response.result;
            var subQuestions = questionProperties["subquestions"];
            if (subQuestions == null)
                return;

            foreach (var subQuestion in subQuestions)
            {
                question.SubQuestions.Add(new SubQuestion()
                {
                    Title = subQuestion.First?["title"]?.ToString(),
                    QuestionText = subQuestion.First?["question"]?.ToString()
                });
            }
        }

        public List<Question> GetQuestionList()
        {
            _client.ClearParameters();
            _client.SetMethod(LimeSurveyMethod.ListQuestions);
            _client.AddParameter(LimeSurveyParameter.SessionKey, SessionKey);
            _client.AddParameter(LimeSurveyParameter.SurveyID, surveyId);
            _client.Post();

            if (HandleClientResponse(_client.Response) != ErrorCode.OK)
                return null;

            var questionList = new List<Question>();
            foreach (var question in (JArray)_client.Response.result)
            {
                var questionObj = JsonUtility.FromJson<Question>(question.ToString());
                SetQuestionProperties(questionObj);
                questionList.Add(questionObj);
            }
            return questionList;
        }

        public void UploadQuestionResponses(IEnumerable<Question> questions, int responseID = -1)
        {
            var responseData = new JObject();
            if (responseID != -1)
                responseData.Add("id", responseID);

            foreach (var question in questions)
            {
                if (question.SubQuestions.Count > 0)
                    foreach (var subQuestion in question.SubQuestions)
                        responseData.Add($"{surveyId}X{question.GID}X{question.ID}{subQuestion.Title}", subQuestion.Answer?.ToString());
                else
                    responseData.Add($"{surveyId}X{question.GID}X{question.ID}", question.Answer?.ToString());

            }

            Debug.Log(responseData);

            _client.ClearParameters();
            _client.SetMethod(LimeSurveyMethod.AddResponse);
            _client.AddParameter(LimeSurveyParameter.SessionKey, SessionKey);
            _client.AddParameter(LimeSurveyParameter.SurveyID, surveyId);
            _client.AddParameter(LimeSurveyParameter.ResponseData, responseData);
            _client.Post();

            if (HandleClientResponse(_client.Response) != ErrorCode.OK)
            {
                Debug.LogError("LimeSurveyManager::UploadQuestionResponses: Unable to upload responses.");
                return;
            }
        }
    }
}
