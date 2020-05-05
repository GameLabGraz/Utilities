using GEAR.Utilities.Attribute;

namespace GEAR.LimeSurvey
{
    public enum LimeSurveyField
    {
        [StringValue("user")] User,
        [StringValue("password")] Password,
        [StringValue("loginlang")] Language,
        [StringValue("action")] Action,
        [StringValue("subaction")] SubAction,
        [StringValue("sid")] Sid,
        [StringValue("the_file")] File,
        [StringValue("noid")] NoId,
        [StringValue("insert")] Insert,
        [StringValue("finalized")] Finalized,
        [StringValue("vvcharset")] Charset
    }

    public enum LimeSurveyLanguage
    {
        [StringValue("default")] Default,
        [StringValue("en")] English,
        [StringValue("de")] German,
    }

    public enum LimeSurveyAction
    {
        [StringValue("login")] Login,
        [StringValue("upload")] Upload,
        [StringValue("vvimport")] Import
    }

    public enum LimeSurveyInsertType
    {
        [StringValue("ignore")] Ignore,
        [StringValue("renumber")] Renumber,
        [StringValue("replace")] Replace
    }

    public enum LimeSurveyCharset
    {
        [StringValue("utf8")] Utf8,
        [StringValue("ascii")] Ascii
    }
}
