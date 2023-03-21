using System.IO;
using UnityEditor;
using XIVEditor.Utils;

namespace XIVEditor
{
    public static class ContextMenuItems
    {
        public const string BASE_MENU = "XIV Utilities";
        public const string CODE_GENERATION_MENU = BASE_MENU + "/Code Generation";
        public const string UPDATE_ALL_CONSTANTS_MENU = CODE_GENERATION_MENU + "/Update All Constants";
        public const string GENERATE_ANIMATION_CONSTANTS_MENU = CODE_GENERATION_MENU + "/Generate Animation Constants";
        public const string GENERATE_PHYSICS_CONSTANTS_MENU = CODE_GENERATION_MENU + "/Generate Physics Constants";
        public const string GENERATE_TAG_CONSTANTS_MENU = CODE_GENERATION_MENU + "/Generate Tag Constants";
        public const string GENERATE_SHADER_CONSTANTS_MENU = CODE_GENERATION_MENU + "/Generate Shader Constants";

        [MenuItem(UPDATE_ALL_CONSTANTS_MENU)]
        public static void UpdateAllConstants()
        {
            File.WriteAllText(FilePaths.ANIMATION_CONSTANTS_FILE, AnimationConstantsGenerator.GetClassString());
            File.WriteAllText(FilePaths.PHYSICS_CONSTANTS_FILE, PhysicsConstantsGenerator.GetClassString());
            File.WriteAllText(FilePaths.TAG_CONSTANTS_FILE, TagConstantsGenerator.GetClassString());
            File.WriteAllText(FilePaths.SHADER_CONSTANTS_FILE, ShaderConstantsGenerator.GetClassString());
            AssetDatabase.Refresh();
        }

        [MenuItem(GENERATE_ANIMATION_CONSTANTS_MENU)]
        public static void GenerateAnimationConstants()
        {
            File.WriteAllText(FilePaths.ANIMATION_CONSTANTS_FILE, AnimationConstantsGenerator.GetClassString());
            AssetDatabase.Refresh();
        }

        [MenuItem(GENERATE_PHYSICS_CONSTANTS_MENU)]
        public static void GeneratePhysicsConstants()
        {
            File.WriteAllText(FilePaths.PHYSICS_CONSTANTS_FILE, PhysicsConstantsGenerator.GetClassString());
            AssetDatabase.Refresh();
        }

        [MenuItem(GENERATE_TAG_CONSTANTS_MENU)]
        public static void GenerateTagConstants()
        {
            File.WriteAllText(FilePaths.TAG_CONSTANTS_FILE, TagConstantsGenerator.GetClassString());
            AssetDatabase.Refresh();
        }

        [MenuItem(GENERATE_SHADER_CONSTANTS_MENU)]
        public static void GenerateShaderConstants()
        {
            File.WriteAllText(FilePaths.SHADER_CONSTANTS_FILE, ShaderConstantsGenerator.GetClassString());
            AssetDatabase.Refresh();
        }

    }

}
