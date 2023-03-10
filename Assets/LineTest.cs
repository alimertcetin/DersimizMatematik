using UnityEngine;
using XIV;
using XIV.XIVMath;

[ExecuteAlways]
public class LineTest : MonoBehaviour
{
    public Transform lineStart;
    public Transform lineEnd;
    public Transform point;
    public float debugRadius = 0.25f;
    public float debugDuration = 0.25f;
    public float threshold = 0.5f;

    // Update is called once per frame
    void Update()
    {
        var lineStartPosition = lineStart.position;
        var lineEndPosition = lineEnd.position;
        var pointPosition = point.position;
        var closestPointOnLine = LineMath.GetClosestPointOnLineSegment(lineStartPosition, lineEndPosition, pointPosition);
        bool isBetween = Vector3.Distance(closestPointOnLine, pointPosition) < threshold;
        Debug.Log(isBetween);

        XIVDebug.DrawSphere(lineStartPosition, debugRadius, Color.green, debugDuration);
        XIVDebug.DrawSphere(lineEndPosition, debugRadius, Color.yellow, debugDuration);
        XIVDebug.DrawSphere(closestPointOnLine, debugRadius, Color.red, debugDuration);
        XIVDebug.DrawLine(lineStartPosition, pointPosition, Color.magenta, debugDuration);
        XIVDebug.DrawLine(lineStartPosition, lineEndPosition, isBetween ? Color.red : Color.blue, debugDuration);
        XIVDebug.DrawLine(pointPosition, closestPointOnLine, Color.cyan, debugDuration);
    }
}
