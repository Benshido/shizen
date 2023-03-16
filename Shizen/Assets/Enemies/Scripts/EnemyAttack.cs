using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyAttack
{
    [SerializeField] string name;
    [SerializeField] int animationIndex;

    [SerializeField] float range = 2;
    [SerializeField] float damage = 2;
    [SerializeField] int maxAmmo = 2;

    public int Ammo { get; private set; }
    /// <summary>
    /// Can be used as cooldown
    /// </summary>
    [SerializeField] float reloadTime = 2;
    [SerializeField] bool hasInfiniteAmmo = false;
    [SerializeField] bool reloadMaxAmmo = false;
    [SerializeField] bool canNotReload = false;
    [Header("Melee settings")]
    [Tooltip("Not required")]
    public Collider MeleeHitArea;
    [Header("Projectile settings")]
    [Tooltip("Not required")]
    public GameObject Projectile;
    public float projectileSpeed = 2;

    private bool reloading = false;

    public string Name { get { return name; } }
    public int AnimationIndex { get { return animationIndex; } }
    public float Range { get { return range; } }
    public float Damage { get { return damage; } }
    public int MaxAmmo { get { return maxAmmo; } }
    public bool HasInfiniteAmmo { get { return hasInfiniteAmmo; } }

    public void FullAmmo()
    {
        Ammo = maxAmmo;
    }

    public void UseAmmo()
    {
        Ammo--;
    }

    public IEnumerator Reload()
    {
        if (!reloading && !canNotReload)
            if (!reloadMaxAmmo && Ammo < maxAmmo || reloadMaxAmmo && Ammo == 0)
            {
                reloading = true;
                yield return new WaitForSeconds(reloadTime);

                if (reloadMaxAmmo) Ammo = maxAmmo;
                else Ammo++;
                reloading = false;
            }
    }
}
