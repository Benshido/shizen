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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            TakeAttack(2);
        }
    }

    public void TakeAttack(float damage)
    {
        hitPoints -= damage;
        //if (hitPoints <= 0) IsAlive = false;
        if (hitPoints <= 0) Destroy(gameObject);
    }
}
