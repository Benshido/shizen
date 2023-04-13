#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyAI))]
public class FOVEditor : Editor
{
    private void OnSceneGUI()
    {

        EnemyAI aI = (EnemyAI)target;
        var pos = aI.transform.position;

        Handles.color = Color.red;
        Handles.DrawWireArc(pos, Vector3.up, Vector3.forward, 360, aI.baseHearingRange);

        Handles.color = Color.cyan;
        Handles.DrawWireArc(pos, Vector3.up, Vector3.forward, 360, aI.baseVisionRange);

        Handles.color = Color.magenta;
        Handles.DrawWireArc(pos, Vector3.up, Vector3.forward, 360, aI.baseHearingRange + aI.hearingAlertedStateIncrease);
        Handles.color = Color.yellow;
        Handles.DrawWireArc(pos, Vector3.up, Vector3.forward, 360, aI.baseVisionRange + aI.visionAlertedStateIncrease);

        Vector3 vAngle_01 = DirectionFromAngle(aI.transform.eulerAngles.y, -aI.visionAngle);
        Vector3 vAngle_02 = DirectionFromAngle(aI.transform.eulerAngles.y, aI.visionAngle);

        Handles.color = Color.white;
        Handles.DrawLine(pos, pos + vAngle_01 * aI.baseVisionRange);
        Handles.DrawLine(pos, pos + vAngle_02 * aI.baseVisionRange);

    }

    private Vector3 DirectionFromAngle(float eulerY, float angleDeg)
    {
        angleDeg += eulerY;
        return new Vector3(Mathf.Sin(angleDeg * Mathf.Deg2Rad), 0, Mathf.Cos(angleDeg * Mathf.Deg2Rad));
    }
}
#endif