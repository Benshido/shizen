using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitsPlayer : MonoBehaviour
{
    bool cooldown = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !cooldown)
        {
            Debug.Log("Player got hit!!!");
            cooldown = true;
            StartCoroutine(HitCooldown(1.0f));
        }
    }

    IEnumerator HitCooldown(float time)
    {
        yield return new WaitForSeconds(time);
        cooldown = false;
        StopAllCoroutines();
    }
}
