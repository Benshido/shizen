using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitsPlayer : MonoBehaviour
{
    bool cooldown = false;
    public float damage = 5;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !cooldown)
        {
            Debug.Log("Player got hit!!!");
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
