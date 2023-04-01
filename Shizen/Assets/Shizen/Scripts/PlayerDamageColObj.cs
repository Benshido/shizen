using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageColObj : MonoBehaviour
{
    [SerializeField] float damage;

    [SerializeField] bool destroyWhenHitHeavyObjects;
    [SerializeField] GameObject destroyThis;
    [SerializeField] float objectWeight;
    public float ObjectWeight { get { return objectWeight; } }
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
        else if (destroyWhenHitHeavyObjects)
        {
            Debug.Log("destr");

            if (other.gameObject.TryGetComponent(out PlayerDamageColObj obj) && obj.ObjectWeight >= ObjectWeight)
            {
                Destroy(destroyThis);
            }
        }
    }
}
