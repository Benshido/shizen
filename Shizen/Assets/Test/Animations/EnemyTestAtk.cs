using UnityEngine;

public class EnemyTestAtk : MonoBehaviour
{
    [SerializeField] float dmg = 5;

    private void OnTriggerEnter(Collider other)
    {
        var charColl = other.gameObject.GetComponent<PlayerHP>();
        if (charColl != null)
        {
            charColl.TakeDamage(dmg);
           // Destroy(gameObject);
        }
    }
}
