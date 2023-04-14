using UnityEngine;

public class AlertGroup : MonoBehaviour
{
    public void Alert()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).GetChild(0).gameObject.GetComponent<EnemyAIController>().alerted)
            {
                transform.GetChild(i).GetChild(0).gameObject.GetComponent<EnemyAIController>().alerted = true;
            }
        }
    }
}
