using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertGroup : MonoBehaviour
{
    public void Alert()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetChild(0).gameObject.GetComponent<EnemyAIController>().AlertWalkpoint();
        }
    }
}
