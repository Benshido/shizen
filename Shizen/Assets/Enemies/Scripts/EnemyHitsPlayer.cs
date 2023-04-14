using System.Collections;
using UnityEngine;

public class EnemyHitsPlayer : MonoBehaviour
{
    bool cooldown = false;
    public float damage = 2;
    public Transform player;

    private int elementLayerIndex = 0;

    private void Start()
    {
        elementLayerIndex = LayerMask.NameToLayer("Elements");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "Projectile" && other.gameObject.layer == elementLayerIndex) Destroy(gameObject);

        if (other.gameObject.tag == "Player" && !cooldown)
        {
            player = FindFirstObjectByType<PlayerHP>().transform;
            player.GetComponent<PlayerHP>().TakeDamage(damage);
            if (gameObject.tag == "Projectile") Destroy(gameObject);
            cooldown = true;
            StartCoroutine(HitCooldown(1.0f));
        }

        if (gameObject.tag == "Projectile") Invoke(nameof(DestroyProjectile), 3f); ;
    }

    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    IEnumerator HitCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        cooldown = false;
        StopAllCoroutines();
    }
}
