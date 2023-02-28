using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using XIV.Utils;

namespace XIV.EditorUtils
{
    public static class AnimationConstantsGenerator
    {
        const string SEPARATOR = "|";
        const string CLASS_NAME = "AnimationConstants";

        public static string GetClassString()
        {
            static string FormatStringFieldValue(string value) => $"\"{value}\"";
            
            // Find all Animator controllers in the project
            string[] guids = AssetDatabase.FindAssets("t:AnimatorController");
            List<AnimatorController> animators = new List<AnimatorController>();
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
                if (controller != null)
                {
                    animators.Add(controller);
                }
            }

            ClassGenerator generator = new ClassGenerator(CLASS_NAME, classModifier: "static");
            
            List<string> animationNames = new List<string>();
            Dictionary<AnimatorControllerParameter, string> parameters = new Dictionary<AnimatorControllerParameter, string>();
            for (int i = 0; i < animators.Count; i++)
            {
                AnimatorController controller = animators[i];
                string controllerName = controller.name.Replace("Animator_", "").Replace("Animator", "");
                for (int j = 0; j < controller.parameters.Length; j++)
                {
                    AnimatorControllerParameter param = controller.parameters[j];
                    if (!parameters.ContainsKey(param))
                    {
                        parameters.Add(param, controllerName + SEPARATOR + param.name);
                    }
                }

                for (int j = 0; j < controller.animationClips.Length; j++)
                {
                    AnimationClip animationClip = controller.animationClips[j];
                    animationNames.Add(controllerName + SEPARATOR + animationClip.name);
                }

                ClassGenerator innerClass = new ClassGenerator(controllerName, classModifier: "static", isInnerClass: true);

                foreach (var name in animationNames)
                {
                    var unpacked = name.Split(SEPARATOR);
                    string animatorName = CleanFieldName(unpacked[0]);
                    string animationName = CleanFieldName(unpacked[1]);
                    innerClass.AddField($"{animatorName}_{animationName}", FormatStringFieldValue(animationName), "string", "const");
                }
                animationNames.Clear();

                foreach (KeyValuePair<AnimatorControllerParameter, string> pair in parameters)
                {
                    var unpacked = pair.Value.Split(SEPARATOR);
                    string animatorName = CleanFieldName(unpacked[0]);
                    string paramName = CleanFieldName(unpacked[1]);
                    string typeSuffix = GetParameterTypeSuffix(pair.Key.type);
                    innerClass.AddField($"{animatorName}_{paramName}_{typeSuffix}", FormatStringFieldValue(paramName), "string", "const");
                }
                parameters.Clear();
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
