using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace XIV.EditorUtils
{
    public static class AnimationConstantsGenerator
    {
        const string SEPERATOR = "|";

        public static string GetClassString()
        {
            // Find all Animator controllers in the project
            string[] guids = AssetDatabase.FindAssets("t:AnimatorController");
            List<AnimatorController> animators = new List<AnimatorController>();
            List<string> animationNames = new List<string>();
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                AnimatorController controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(path);
                if (controller != null)
                {
                    animators.Add(controller);
                }
            }

            // Find all Animator parameters used in the controllers
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
                        parameters.Add(param, controllerName + SEPERATOR + param.name);
                    }
                }

                for (int j = 0; j < controller.animationClips.Length; j++)
                {
                    AnimationClip animationClip = controller.animationClips[j];
                    animationNames.Add(controllerName + SEPERATOR + animationClip.name);
                }
            }

            // Generate the AnimationConstants class
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("public static class AnimationConstants");
            sb.AppendLine("{");

            sb.AppendLine("\t// Animation names");
            // Loop through the animations and add them to the class as static fields
            foreach (var name in animationNames)
            {
                var unpacked = name.Split(SEPERATOR);
                string animatorName = CleanFieldName(unpacked[0]);
                string animationName = CleanFieldName(unpacked[1]);
                sb.AppendLine($"\tpublic const string {animatorName}_{animationName} = \"{animationName}\";");
            }

            sb.AppendLine("\t// Animator parameters");
            foreach (KeyValuePair<AnimatorControllerParameter, string> pair in parameters)
            {
                var unpacked = pair.Value.Split(SEPERATOR);
                string animatorName = CleanFieldName(unpacked[0]);
                string paramName = CleanFieldName(unpacked[1]);
                string typeSuffix = GetParameterTypeSuffix(pair.Key.type);
                sb.AppendLine($"\tpublic const string {animatorName}_{paramName}_{typeSuffix} = \"{paramName}\";");
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        private static string CleanFieldName(string fieldName)
        {
            // Replace spaces and other characters with underscores
            return fieldName.Replace(" ", "_").Replace("-", "_");
        }

        private static string GetParameterTypeSuffix(AnimatorControllerParameterType type)
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
