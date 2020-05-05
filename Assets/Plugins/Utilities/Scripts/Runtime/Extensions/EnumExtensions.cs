using System;
using System.Linq;
using System.Reflection;
using GEAR.Utilities.Attribute;

namespace GEAR.Utilities.Extensions
{
    public static class EnumExtensions
    {
        public static string GetStringValue(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var memberInfo = enumType.GetMember(enumValue.ToString());
            var enumValueMemberInfo = memberInfo.FirstOrDefault(m => m.DeclaringType == enumType);
            var stingValueAttribute = enumValueMemberInfo?.GetCustomAttribute<StringValue>();
            return stingValueAttribute == null ? enumType.ToString() : stingValueAttribute.Value;
        }
    }
}