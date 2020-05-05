using System;
using GEAR.Utilities.Extensions;
using UnityEngine;

namespace GEAR.LimeSurvey.Extensions
{
    public static class WWWFormExtensions
    {
        public static void AddField(this WWWForm form, LimeSurveyField field, Enum value)
        {
            form.AddField(field.GetStringValue(), value.GetStringValue());
        }

        public static void AddField(this WWWForm form, string field, Enum value)
        {
            form.AddField(field, value.GetStringValue());
        }

        public static void AddField(this WWWForm form, LimeSurveyField field, string value)
        {
            form.AddField(field.GetStringValue(), value);
        }

        public static void AddBinaryData(this WWWForm form, LimeSurveyField field, byte[] data)
        {
            form.AddBinaryData(field.GetStringValue(), data);
        }
    }
}
