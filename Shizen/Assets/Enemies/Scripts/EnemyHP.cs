using UnityEngine;

public class EnemyHP : MonoBehaviour
{

    [SerializeField] float maxHitPoints = 5;

    private float hitPoints = 1;
    public float HitPoints { get { return hitPoints; } }

    public bool IsAlive { get; private set; }

    void Start()
    {
        hitPoints = maxHitPoints;

        IsAlive = true;
    }

    public void TakeAttack(float damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0) IsAlive = false;

        if (gameObject.transform.parent != null && gameObject.transform.parent.gameObject.GetComponent<EnemyAIController>())
        {
            gameObject.transform.parent.gameObject.GetComponent<EnemyAIController>().RespondToAttack(IsAlive);
        }
    }
}
