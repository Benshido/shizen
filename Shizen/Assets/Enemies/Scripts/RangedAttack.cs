using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    public GameObject projectile;
    public Transform projectileTransform;

    public void ThrowProjectile(Transform player)
    {
        Rigidbody rb = Instantiate(projectile, projectileTransform.position, Quaternion.identity).GetComponent<Rigidbody>();
        Vector3 direction = (player.position - projectileTransform.position).normalized;
        float angle = Vector3.Angle(direction, Vector3.forward);

        if (direction.x < 0) angle = -angle;
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        rb.rotation = rotation;
        rb.AddForce(direction * 32f, ForceMode.Impulse);
        //rb.AddForce(transform.up * 8f, ForceMode.Impulse);
    }
}
