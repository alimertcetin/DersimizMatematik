using UnityEditorInternal;
using XIV.Core.Utils;

namespace XIV.XIVEditor.CodeGeneration
{
    public static class PhysicsConstantsGenerator
    {
        const string CLASS_NAME = "PhysicsConstants";

        public static string GetClassString()
        {
            ClassGenerator generator = new ClassGenerator(CLASS_NAME, classModifier: "static");
            generator.Use(nameof(UnityEngine));

            var layers = InternalEditorUtility.layers;

            for (int i = 0; i < layers.Length; i++)
            {
                var fieldName = CleanFieldName(layers[i]);
                var fieldValue = FormatStringFieldValue(layers[i]);
                generator.AddField(fieldName, fieldValue, "string", "const");
                generator.AddField(fieldName + "Layer", ToLayerMask(fieldValue), "int", "static readonly");
            }


            return generator.EndClass();
        }

        static string FormatStringFieldValue(string value) => $"\"{value}\"";
        static string ToLayerMask(string field) => $"LayerMask.NameToLayer({field})";

        static string CleanFieldName(string fieldName)
        {
            // Replace spaces and other characters with underscores
            return fieldName.Replace(" ", "_").Replace("-", "_");
        }
    }
}