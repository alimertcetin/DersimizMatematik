using LessonIsMath.DoorSystems;
using UnityEditor;

namespace Assets.Scripts.CustomInspector
{
    [CustomEditor(typeof(Door))]
    public class DoorInspector : Editor
    {
        SerializedProperty useAirthmeticOperationSP;

        private void OnEnable()
        {
            useAirthmeticOperationSP = serializedObject.FindProperty("useArithmeticOperation");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            if (useAirthmeticOperationSP.boolValue == false)
            {
                DrawPropertiesExcluding(serializedObject, "arithmeticOperation", "maxValueOfAnswer");
            }
            else
            {
                DrawPropertiesExcluding(serializedObject);
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }
    }
}