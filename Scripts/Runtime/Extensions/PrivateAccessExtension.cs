using System.Reflection;

namespace GEAR.Gadgets.Extensions
{
    public static class PrivateAccessExtension
    {
        public static T GetFieldValue<T>(this object obj, string name)
        {
            var field = obj.GetType()
                .GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)field?.GetValue(obj);
        }

        public static void SetFieldValue<T>(this object obj, string name, T value)
        {
            var field = obj.GetType()
                .GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(obj, value);
        }
        
        public static T GetBaseFieldValue<T>(this object obj, string name)
        {
            var field = obj.GetType().BaseType
                .GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)field?.GetValue(obj);
        }

        public static void SetBaseFieldValue<T>(this object obj, string name, T value)
        {
            var field = obj.GetType().BaseType
                .GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(obj, value);
        }
        
        public static void CallMethod(this object obj, string name, object[] args)
        {
            var method = obj.GetType().GetMethod(name, 
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);

            method.Invoke(obj, args);
        }
        
        public static void CallBaseMethod(this object obj, string name, object[] args)
        {
            // var tmp = GetType().BaseType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod).Aggregate("", (current, methodInfo) => current + (methodInfo.Name + "\n"));
            var method = obj.GetType().BaseType.GetMethod(name, 
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);

            method.Invoke(obj, args);
        }
    }
}
