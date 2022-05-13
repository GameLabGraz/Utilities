using GEAR.Gadgets.Attribute;

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
}
