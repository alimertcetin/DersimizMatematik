using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using XIV.Utils;

namespace XIVEditor.Utils
{
    public static class AnimationConstantsGenerator
    {
        const string CLASS_NAME = "AnimationConstants";

        public static string GetClassString()
        {
            ClassGenerator generator = new ClassGenerator(CLASS_NAME, classModifier: "static");
            generator.Use(nameof(UnityEngine));
            // Find all Animator controllers in the project
            string[] guids = AssetDatabase.FindAssets("t:AnimatorController");
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
                if (controller == null) continue;
                
                string controllerName = controller.name.Replace("Animator_", "").Replace("Animator", "");
                string animatorName = CleanFieldName(controllerName);

                int clipLength = controller.animationClips.Length;
                int parameterLength = controller.parameters.Length;

                ClassGenerator innerClass = new ClassGenerator(controllerName, classModifier: "static", isInnerClass: true);
                if (clipLength != 0) WriteClips(controller, animatorName, innerClass);
                if (parameterLength != 0) WriteParamaters(controller, animatorName, innerClass);
                // Controller must have at least one Layer
                WriteLayers(controller, animatorName, innerClass);
                
                if (clipLength == 0 && parameterLength == 0)
                {
                    Debug.LogWarning("There is no clip and parameter in " + controller.name);
                }

                generator.AddInnerClass(innerClass);
            }


            return generator.EndClass();
        }

        static void WriteClips(AnimatorController controller, string animatorName, ClassGenerator innerClass)
        {
            ClassGenerator clipClass = new ClassGenerator("Clips", classModifier: "static", isInnerClass: true);
            
            for (int j = 0; j < controller.animationClips.Length; j++)
            {
                AnimationClip animationClip = controller.animationClips[j];
                string animationName = CleanFieldName(animationClip.name);
                var fieldName = $"{animatorName}_{animationName}";
                var fieldValue = FormatStringFieldValue(animationName);
                clipClass.AddField(fieldName, fieldValue, "string", "const");
                clipClass.AddField(fieldName + "Hash", ToHashField(fieldValue), "int", "static readonly");
            }

            innerClass.AddInnerClass(clipClass);
        }

        static void WriteParamaters(AnimatorController controller, string animatorName, ClassGenerator innerClass)
        {
            ClassGenerator paramaterClass = new ClassGenerator("Parameters", classModifier: "static", isInnerClass: true);
            
            for (int j = 0; j < controller.parameters.Length; j++)
            {
                AnimatorControllerParameter param = controller.parameters[j];
                string paramName = CleanFieldName(param.name);
                string typeSuffix = param.type.ToString();
                var fieldName = $"{animatorName}_{paramName}_{typeSuffix}";
                var fieldValue = FormatStringFieldValue(paramName);
                paramaterClass.AddField(fieldName, fieldValue, "string", "const");
            }

            innerClass.AddInnerClass(paramaterClass);
        }

        static void WriteLayers(AnimatorController controller, string animatorName, ClassGenerator innerClass)
        {
            ClassGenerator layerClass = new ClassGenerator("Layers", classModifier: "static", isInnerClass: true);
            
            for (int j = 0; j < controller.layers.Length; j++)
            {
                AnimatorControllerLayer layer = controller.layers[j];
                var layerName = CleanFieldName(layer.name);
                if (layerName.ToLower().Contains("layer") == false) layerName += "_Layer";
                layerClass.AddField($"{animatorName}_{layerName}", j.ToString(), "int", "const");
            }

            innerClass.AddInnerClass(layerClass);
        }

        static string FormatStringFieldValue(string value) => $"\"{value}\"";
        static string ToHashField(string field) => $"Animator.StringToHash({field})";

        static string CleanFieldName(string fieldName)
        {
            // Replace spaces and other characters with underscores
            return fieldName.Replace(" ", "_").Replace("-", "_");
        }
    }

}
