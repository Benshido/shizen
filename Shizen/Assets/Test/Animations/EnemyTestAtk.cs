using UnityEngine;

public class EnemyTestAtk : MonoBehaviour
{
    [SerializeField] float dmg = 5;

    //Do not use this for final result, this is just a test
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerHP>(out var charColl))
        {
            charColl.TakeDamage(dmg);
           // Destroy(gameObject);
        }
    }
}
