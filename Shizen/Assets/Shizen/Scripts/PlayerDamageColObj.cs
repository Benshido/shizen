using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageColObj : MonoBehaviour
{
    [SerializeField] float damage;
   // [SerializeField] float knockbackForce;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.TryGetComponent(out EnemyHP hp))
            {
                hp.TakeAttack(damage);
            }
        }
    }
}
