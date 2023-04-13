using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyAttack
{
    [SerializeField] string name;
    [SerializeField] int animationIndex;
    [SerializeField] public float cooldown;
    //public Collider MeleeHitArea;

    private bool reloading = false;

    public string Name { get { return name; } }
    public int AnimationIndex { get { return animationIndex; } }

    public IEnumerator Cooldown()
    {
        if (!reloading)
        {
            reloading = true;
            yield return new WaitForSeconds(cooldown);
            reloading = false;
        }
    }
}
