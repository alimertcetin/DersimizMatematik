using System;
using System.Reflection;
using XIV.Core.Extensions;

namespace XIV.XIVEditor.Utils
{
    public static class ReflectionUtils
    {
        static readonly BindingFlags DefaultFieldBindingFlags = 
            BindingFlags.Instance | 
            BindingFlags.NonPublic | 
            BindingFlags.Public | 
            BindingFlags.GetField;
        
        public static void SetField(string fieldName, object instance, object fieldValue, BindingFlags bindingFlags)
        {
            var type = instance.GetType();
            FieldInfo propertyInfo = type.GetField(fieldName, bindingFlags);
            propertyInfo.SetValue(instance, fieldValue);
        }
        
        public static void SetField(string fieldName, object instance, object fieldValue)
        {
            SetField(fieldName, instance, fieldValue, DefaultFieldBindingFlags);
        }

        public static object GetFieldValue(string fieldName, object instance)
        {
            var type = instance.GetType();
            FieldInfo propertyInfo = type.GetField(fieldName, DefaultFieldBindingFlags);
            return propertyInfo.GetValue(instance);
        }

        /// <typeparam name="T">Type of the value</typeparam>
        public static T GetFieldValue<T>(string fieldName, object instance)
        {
            return (T)GetFieldValue(fieldName, instance);
        }
        
        public static void SetProperty(string fieldName, object instance, object fieldValue, BindingFlags bindingFlags)
        {
            var type = instance.GetType();
            PropertyInfo propertyInfo = type.GetProperty(fieldName, bindingFlags);
            propertyInfo.SetValue(instance, fieldValue, null);
        }
        
        public static void SetProperty(string fieldName, object instance, object fieldValue)
        {
            SetProperty(fieldName, instance, fieldValue, DefaultFieldBindingFlags);
        }

        public static MethodInfo[] GetMethods<TAttribute>(Type type) where TAttribute : Attribute
        {
            return type.GetMethods(DefaultFieldBindingFlags).Split((m) => CustomAttributeExtensions.GetCustomAttribute<TAttribute>((MemberInfo)m) != null);
        }
    }
}