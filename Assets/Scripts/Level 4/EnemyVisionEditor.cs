//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(EnemyVision))]
using UnityEngine;

public class EnemyVisionEditor : MonoBehaviour
{
    //private void OnSceneGUI()
    //{
    //    EnemyVision ev = (EnemyVision)target;

    //    Handles.color = Color.white;
    //    Handles.DrawWireArc(ev.transform.position, Vector3.up, Vector3.forward, 360f, ev.distance);

    //    Vector3 viewAngleL = DirectionFromAngle(ev.transform.eulerAngles.y, -ev.angle / 2);
    //    Vector3 viewAngleR = DirectionFromAngle(ev.transform.eulerAngles.y, ev.angle / 2);

    //    Handles.color = Color.yellow;
    //    Handles.DrawLine(ev.transform.position, ev.transform.position + viewAngleL * ev.distance);
    //    Handles.DrawLine(ev.transform.position, ev.transform.position + viewAngleR * ev.distance);

    //    if (ev.playerInVision)
    //    {
    //        Handles.color = Color.green;
    //        Handles.DrawLine(ev.transform.position, ev.player.transform.position);
    //    }
    //}

    //private Vector3 DirectionFromAngle (float eulerY, float angleInDegree)
    //{
    //    angleInDegree += eulerY;

    //    return new Vector3(Mathf.Sin(angleInDegree * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegree * Mathf.Deg2Rad));
    //}
}
