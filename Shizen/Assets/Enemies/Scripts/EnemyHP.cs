using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{

    [SerializeField] float maxHitPoints = 5;
    public Slider hitPointsValue;

    private float hitPoints = 1;
    public float HitPoints { get { return hitPoints; } }

    public bool IsAlive { get; private set; }

    void Start()
    {
        hitPoints = maxHitPoints;
        hitPointsValue.maxValue = maxHitPoints;
        hitPointsValue.value = hitPoints;
        IsAlive = true;
    }

    public void TakeAttack(float damage)
    {
        hitPoints -= damage;
        hitPointsValue.value = hitPoints;
        if (hitPoints <= 0) IsAlive = false;

        if (gameObject.transform.parent != null && gameObject.transform.parent.gameObject.GetComponent<EnemyAIController>())
        {
            gameObject.transform.parent.gameObject.GetComponent<EnemyAIController>().RespondToAttack(IsAlive);
        }
    }
}
