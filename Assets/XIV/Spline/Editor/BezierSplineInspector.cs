using UnityEditor;
using UnityEngine;

namespace XIV.Spline.XIVEditor
{
    [CustomEditor(typeof(BezierSpline))]
    public class BezierSplineInspector : Editor
    {
        const float focusDistance = 4f;
        const float directionScale = 0.5f;
        const int stepsPerCurve = 10;

        const float handleSize = 0.06f;
        const float pickSize = 0.1f;

        int selectedIndex = -1;

        BezierSpline bezierSpline;
        Transform bezierSplineTransform;
        Quaternion handleRotation;

        float t;

        void OnEnable()
        {
            bezierSpline = target as BezierSpline;
            bezierSplineTransform = bezierSpline.transform;
            bezierSplineTransform.hideFlags = HideFlags.NotEditable | HideFlags.HideInInspector;
            
            bezierSpline.CalculateSplineLength();
        }

        void OnDisable()
        {
            if (bezierSplineTransform != null)
            {
                bezierSplineTransform.hideFlags = HideFlags.None;
                Tools.hidden = false;
            }
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            t = EditorGUILayout.Slider(t, 0, 1);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(bezierSpline, "Move Point");
                SceneView.lastActiveSceneView.Repaint();
            }
            
            GUILayout.Label("Spline Length : " + bezierSpline.Length);
            
            if (selectedIndex >= 0 && selectedIndex < bezierSpline.PointCount)
            {
                DrawSelectedPointInspector();
                Tools.hidden = true;
            }
            else
            {
                Tools.hidden = false;
            }

            if (GUILayout.Button("Add Curve"))
            {
                Undo.RecordObject(bezierSpline, "Add Curve");
                bezierSpline.AddCurve();
                EditorUtility.SetDirty(bezierSpline);
            }

            if (GUILayout.Button("Remove Curve"))
            {
                Undo.RecordObject(bezierSpline, "Remove Curve");
                bezierSpline.RemoveCurve(selectedIndex);
                EditorUtility.SetDirty(bezierSpline);
            }

            if (GUILayout.Button("Clear Selection"))
            {
                if (selectedIndex != -1)
                {
                    selectedIndex = -1;
                    SceneView.RepaintAll();
                }
            }
        }


        void DrawSelectedPointInspector()
        {
            GUILayout.Label("Selected Point : " + selectedIndex);
            EditorGUI.BeginChangeCheck();
            Vector3 point = EditorGUILayout.Vector3Field("Position", bezierSpline.GetPoint(selectedIndex));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(bezierSpline, "Move Point");
                EditorUtility.SetDirty(bezierSpline);
                bezierSpline.SetPoint(selectedIndex, point);
            }

            if (GUILayout.Button("Focus"))
            {
                // Can't set transform of camera :(
                // It internally updates every frame:
                //      cam.position = pivot + rotation * new Vector3(0, 0, -cameraDistance)
                // Info: https://forum.unity.com/threads/moving-scene-scene_view-camera-from-editor-script.64920/#post-3388397
                // But we can align it to an object! Source: http://answers.unity.com/answers/256969/scene_view.html
                point = bezierSplineTransform.TransformPoint(point);
                var sceneView = SceneView.lastActiveSceneView;
                var camera = sceneView.camera;
                var cameraPosition = camera.transform.position;
                var directionToPoint = point - cameraPosition;
            
                var targetPos = point;
                targetPos -= directionToPoint.normalized * focusDistance;
                
                sceneView.orthographic = false;
 
                camera.transform.position = targetPos;
                camera.transform.rotation = Quaternion.LookRotation(directionToPoint.normalized);
                sceneView.AlignViewToObject(camera.transform);
            }
            
            GUILayout.Space(10f);
        }

        void OnSceneGUI()
        {
            handleRotation = Tools.pivotRotation == PivotRotation.Local ? bezierSplineTransform.rotation : Quaternion.identity;
            
            DrawPointAtT();

            Vector3 p0 = ShowPoint(0, false);
            for (int i = 1; i < bezierSpline.PointCount; i += 3)
            {
                Vector3 p1 = ShowPoint(i, true);
                Vector3 p2 = ShowPoint(i + 1, true);
                Vector3 p3 = ShowPoint(i + 2, false);

                Handles.color = Color.gray;
                Handles.DrawLine(p0, p1);
                Handles.DrawLine(p2, p3);

                Handles.DrawBezier(p0, p3, p1, p2, Color.white, null, 2f);
                p0 = p3;
            }

            ShowDirections();
        }

        void DrawPointAtT()
        {
            var point = bezierSplineTransform.TransformPoint(bezierSpline.GetPoint(t));
            DrawSphere(point, .05f, Color.magenta, 30);
        }

        const float TAU = 6.283185307179586f;

        void DrawSphere(Vector3 position, float radius, Color color, int steps = 10)
        {
            int sphereStep = 10;
            for (int i = 0; i < sphereStep; i++)
            {
                float radian = (float)i / sphereStep * TAU;
                var dir = Vector3.RotateTowards(Vector3.forward, Vector3.back, radian, radian);
                DrawCircle(position, dir.normalized, radius, color, steps);
            }
        }
        void DrawCircle(Vector3 position, Vector3 lookDir, float radius, Color color, int steps = 10)
        {
            DrawCircle(position, Quaternion.LookRotation(lookDir), radius, color, steps);
        }
        void DrawCircle(Vector3 position, Quaternion rotation, float radius, Color color, int steps = 10)
        {
            Handles.color = color;
            for (int i = 0; i < steps; i++)
            {
                float p0Radian = (float)(i - 1) / steps * TAU;
                float p1Radian = (float)i / steps * TAU;
                Vector3 p0 = new Vector3
                {
                    x = Mathf.Cos(p0Radian) * radius,
                    y = Mathf.Sin(p0Radian) * radius,
                };
                Vector3 p1 = new Vector3
                {
                    x = Mathf.Cos(p1Radian) * radius,
                    y = Mathf.Sin(p1Radian) * radius,
                };
                p0 = position + rotation * p0;
                p1 = position + rotation * p1;
                Handles.DrawLine(p0, p1);
            }
        }

        void ShowDirections()
        {
            Handles.color = Color.green;
            Vector3 point = bezierSplineTransform.TransformPoint(bezierSpline.GetPoint(0f));
            Handles.DrawLine(point, point + bezierSpline.GetDirection(0f) * directionScale);
            int steps = stepsPerCurve * bezierSpline.CurveCount;
            for (int i = 1; i <= steps; i++)
            {
                float t = i / (float)steps;
                point = bezierSplineTransform.TransformPoint(bezierSpline.GetPoint(t));
                Handles.DrawLine(point, point + bezierSpline.GetDirection(t) * directionScale);
            }
        }

        Vector3 ShowPoint(int index, bool isAnchor)
        {
            Vector3 point = bezierSplineTransform.TransformPoint(bezierSpline.GetPoint(index));

            float size = HandleUtility.GetHandleSize(point);
            Handles.color = isAnchor ? Color.blue : Color.white;
            if (Handles.Button(point, handleRotation, size * handleSize, size * pickSize, Handles.DotHandleCap))
            {
                selectedIndex = index;
                Repaint();
            }

            if (selectedIndex == index)
            {
                if (isAnchor == false)
                {
                    MoveWithAnchors(index);
                    return bezierSplineTransform.TransformPoint(bezierSpline.GetPoint(index));
                }
                EditorGUI.BeginChangeCheck();
                point = Handles.DoPositionHandle(point, handleRotation);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(bezierSpline, "Move Point");
                    EditorUtility.SetDirty(bezierSpline);
                    bezierSpline.SetPoint(index, bezierSplineTransform.InverseTransformPoint(point));
                }
            }

            return point;
        }


        void MoveWithAnchors(int controlIndex)
        {
            int previousIndex = controlIndex - 1;
            int nextIndex = controlIndex + 1;
            bool isPreviousAvailable = previousIndex > 0;
            bool isNextAvailable = nextIndex < bezierSpline.PointCount;
            
            Vector3 controlPoint = bezierSplineTransform.TransformPoint(bezierSpline.GetPoint(controlIndex));

            Vector3 diffPrevious = Vector3.zero;
            if (isPreviousAvailable)
            {
                Vector3 previousAnchor = bezierSplineTransform.TransformPoint(bezierSpline.GetPoint(previousIndex));
                diffPrevious = previousAnchor - controlPoint;
            }

            Vector3 diffNext = Vector3.zero;
            if (isNextAvailable)
            {
                Vector3 nextAnchor = bezierSplineTransform.TransformPoint(bezierSpline.GetPoint(nextIndex));
                diffNext = nextAnchor - controlPoint;
            }

            EditorGUI.BeginChangeCheck();
            controlPoint = Handles.DoPositionHandle(controlPoint, handleRotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(bezierSpline, "Move Point");
                EditorUtility.SetDirty(bezierSpline);

                if (isPreviousAvailable)
                {
                    var previousAnchorPos = controlPoint + diffPrevious;
                    bezierSpline.SetPoint(previousIndex, bezierSplineTransform.InverseTransformPoint(previousAnchorPos));
                }

                if (isNextAvailable)
                {
                    var nextAnchorPos = controlPoint + diffNext;
                    bezierSpline.SetPoint(nextIndex, bezierSplineTransform.InverseTransformPoint(nextAnchorPos));
                }
                
                bezierSpline.SetPoint(controlIndex, bezierSplineTransform.InverseTransformPoint(controlPoint));
            }
        }
    }
}