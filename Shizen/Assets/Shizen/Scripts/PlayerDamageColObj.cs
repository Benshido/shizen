using System.Collections;
using UnityEngine;

public class PlayerDamageColObj : MonoBehaviour
{
    [SerializeField] float damage;

    [SerializeField] bool destroyWhenHitHeavyObjects;
    [SerializeField] bool destroyOnEnemyHit;
    [SerializeField] float enemyHitDestrDelay = 0;
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
                if (destroyOnEnemyHit) StartCoroutine(Destroying());
            }
        }
        else if (destroyWhenHitHeavyObjects)
        {

            if (other.gameObject.TryGetComponent(out PlayerDamageColObj obj) && obj.ObjectWeight >= ObjectWeight)
            {
                Destroy(destroyThis);
            }
        }
    }

    private IEnumerator Destroying()
    {
        yield return new WaitForSecondsRealtime(enemyHitDestrDelay);
        Destroy(destroyThis);
    }
}
