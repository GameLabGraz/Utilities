using UnityEngine;
using System.Net;
using System.Collections.Generic;
using GameLabGraz.LimeSurvey.Data;
using GameLabGraz.LimeSurvey.Extensions;
using Unity.Plastic.Newtonsoft.Json.Linq;

namespace GameLabGraz.LimeSurvey
{
    public class LimeSurveyUploader : MonoBehaviour
    {
        [Header("Login")]
        [SerializeField] private string url;
        [SerializeField] private string user;
        [SerializeField] private string password;
        [SerializeField] private string surveyId;

        private JsonRpcClient _client;

        public string SessionKey { get; private set; }


        private void Start()
        {
            _client = new JsonRpcClient(url);
            Login();
            GetQuestions();
        }

        private void Login()
        {
            _client.ClearParameters();
            _client.SetMethod(LimeSurveyMethod.GetSessionKey);
            _client.AddParameter(LimeSurveyParameter.UserName, user);
            _client.AddParameter(LimeSurveyParameter.Password, password);
            _client.Post();

            if (HandleClientResponse(_client.Response) != ErrorCode.OK)
                return;

            var response = _client.Response.result.ToString();
            if (response.Contains("\"status\""))
            {
                Debug.LogError("LimeSurveyUploader::Login: Invalid user name or password.");
            }
            else
            {
                SessionKey = response;
                Debug.Log($"LimeSurveyUploader::Login: Login successfully - Session Key: {SessionKey}");
            }
        }

        private ErrorCode HandleClientResponse(JsonRpcResponse response)
        {
            if (_client.Response.StatusCode != HttpStatusCode.OK)
            {
                Debug.LogError($"LimeSurveyUploader::HandleClientResponse: Error - HTTP Status Code: {_client.Response.StatusCode}");
                return ErrorCode.HttpStatusError;
            }
            else if (_client.Response.error != null)
            {
                Debug.LogError($"LimeSurveyUploader::HandleClientResponse: Error: {_client.Response.error}");
                return ErrorCode.ResponseError;
            }
            return ErrorCode.OK;
        }

        public List<Question> GetQuestions()
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
                Debug.Log(question);


                var questionObj = JsonUtility.FromJson<Question>(question.ToString());
                questionList.Add(questionObj);
            }
            


            return questionList;
        }

    }
}
