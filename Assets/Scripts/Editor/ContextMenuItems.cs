using System.IO;
using UnityEditor;

namespace XIV.EditorUtils
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
