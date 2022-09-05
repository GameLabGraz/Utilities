using System;
using GEAR.Gadgets.Attribute;
using UnityEngine.Events;

namespace GameLabGraz.LimeSurvey
{
    public enum ErrorCode
    {
        OK,
        HttpStatusError,
        ResponseError
    }

    public enum LimeSurveyMethod
    {
        [StringValue("get_session_key")] GetSessionKey,
        [StringValue("list_groups")] ListGroups,
        [StringValue("list_questions")] ListQuestions,
        [StringValue("get_question_properties")] GetQuestionProperties,
        [StringValue("add_response")] AddResponse
    }

    public enum LimeSurveyParameter
    {
        [StringValue("username")] UserName,
        [StringValue("password")] Password,
        [StringValue("sSessionKey")] SessionKey,
        [StringValue("iSurveyID")] SurveyID,
        [StringValue("iGroupID")] GroupID,
        [StringValue("iQuestionID")] QuestionID,
        [StringValue("aResponseData")] ResponseData
    }
    
    [Serializable] public class SubmissionEvent : UnityEvent<int>{}
    [Serializable] public class ErrorEvent : UnityEvent<string, string>{}
    [Serializable] public class WarningEvent : UnityEvent<string, string>{}
    [Serializable] public class LoginEvent : UnityEvent<string>{}
    
    [Serializable] public class SetAnswerEvent : UnityEvent<bool>{}
}
