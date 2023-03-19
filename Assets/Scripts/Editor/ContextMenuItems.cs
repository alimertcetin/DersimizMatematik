using System.IO;
using UnityEditor;
using XIVEditor.Utils;

namespace XIVEditor
{
    public static class ContextMenuItems
    {
        public const string CodeGenerationMenu = "CodeGeneration";
        public const string GenerateAnimationConstantsMenu = CodeGenerationMenu + "/Generate Animation Constants";

        [MenuItem(GenerateAnimationConstantsMenu)]
        public static void GenerateAnimationConstants()
        {
            var animationConstants = AnimationConstantsGenerator.GetClassString();
            File.WriteAllText(FilePaths.AnimationConstantsFile, animationConstants);
            AssetDatabase.Refresh();
        }

    }

}
