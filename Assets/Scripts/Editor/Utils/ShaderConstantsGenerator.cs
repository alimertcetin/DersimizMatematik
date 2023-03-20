using UnityEditor;
using UnityEngine;
using XIV.Utils;

namespace XIVEditor.Utils
{
    public static class ShaderConstantsGenerator
    {
        const string CLASS_NAME = "ShaderConstants";

        public static string GetClassString()
        {
            var guids = AssetDatabase.FindAssets("t: Shader", new[] { "Assets" });
            
            ClassGenerator generator = new ClassGenerator(CLASS_NAME, classModifier: "static");
            generator.Use(nameof(UnityEngine));

            for (var i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                var shader = AssetDatabase.LoadAssetAtPath<Shader>(path);

                var shaderClassName = shader.name.Replace(" ", "")
                    .Replace("-", "_")
                    .Replace("/", "_")
                    .Replace("(","")
                    .Replace(")","");

                ClassGenerator innerClass = new ClassGenerator(shaderClassName, classModifier: "static", isInnerClass: true);
                int propertyCount = shader.GetPropertyCount();
                for (int j = 0; j < propertyCount; j++)
                {
                    var propertyName = shader.GetPropertyName(j);
                    var propertyType = shader.GetPropertyType(j);
                    var fieldValue = FormatStringFieldValue(propertyName);
                    var fieldName = CleanFieldName($"{propertyName}_{propertyType}").TrimStart('_');

                    innerClass.AddField(fieldName, fieldValue, "string", "const");
                    innerClass.AddField(fieldName + "ID", ToPropertyID(FormatStringFieldValue(propertyName)), "int", "static readonly");
                }
                
                generator.AddInnerClass(innerClass);
            }

            return generator.EndClass();
        }

        static string FormatStringFieldValue(string value) => $"\"{value}\"";
        static string ToPropertyID(string field) => $"Shader.PropertyToID({field})";

        static string CleanFieldName(string fieldName)
        {
            // Replace spaces and other characters with underscores
            return fieldName.Replace(" ", "_").Replace("-", "_");
        }
    }
}