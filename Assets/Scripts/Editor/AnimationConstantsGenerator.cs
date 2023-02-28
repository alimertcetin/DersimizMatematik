using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using XIV.Utils;

namespace XIV.EditorUtils
{
    public static class AnimationConstantsGenerator
    {
        const string CLASS_NAME = "AnimationConstants";

        public static string GetClassString()
        {
            static string FormatStringFieldValue(string value) => $"\"{value}\"";
            
            ClassGenerator generator = new ClassGenerator(CLASS_NAME, classModifier: "static");
            // Find all Animator controllers in the project
            string[] guids = AssetDatabase.FindAssets("t:AnimatorController");
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
                if (controller == null) continue;
                
                string controllerName = controller.name.Replace("Animator_", "").Replace("Animator", "");
                string animatorName = CleanFieldName(controllerName);
                
                ClassGenerator innerClass = new ClassGenerator(controllerName, classModifier: "static", isInnerClass: true);
                // Animations
                for (int j = 0; j < controller.animationClips.Length; j++)
                {
                    AnimationClip animationClip = controller.animationClips[j];
                    string animationName = CleanFieldName(animationClip.name);
                    innerClass.AddField($"{animatorName}_{animationName}", FormatStringFieldValue(animationName), "string", "const");
                }
                
                // Parameters
                for (int j = 0; j < controller.parameters.Length; j++)
                {
                    AnimatorControllerParameter param = controller.parameters[j];
                    string paramName = CleanFieldName(param.name);
                    string typeSuffix = GetParameterTypeSuffix(param.type);
                    innerClass.AddField($"{animatorName}_{paramName}_{typeSuffix}", FormatStringFieldValue(paramName), "string", "const");
                }
                
                // Layers
                for (int j = 0; j < controller.layers.Length; j++)
                {
                    AnimatorControllerLayer layer = controller.layers[j];
                    var layerName = CleanFieldName(layer.name);
                    if (layerName.ToLower().Contains("layer") == false) layerName += "_Layer";
                    innerClass.AddField($"{animatorName}_{layerName}", j.ToString(), "int", "const");
                }
                
                generator.AddInnerClass(innerClass);
            }


            return generator.EndClass();
        }

        static string CleanFieldName(string fieldName)
        {
            // Replace spaces and other characters with underscores
            return fieldName.Replace(" ", "_").Replace("-", "_");
        }

        static string GetParameterTypeSuffix(AnimatorControllerParameterType type)
        {
            switch (type)
            {
                case AnimatorControllerParameterType.Bool:
                    return "Bool";
                case AnimatorControllerParameterType.Float:
                    return "Float";
                case AnimatorControllerParameterType.Int:
                    return "Int";
                case AnimatorControllerParameterType.Trigger:
                    return "Trigger";
                default:
                    return "";
            }
        }
    }

}
