using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosCoreAnimator : MonoBehaviour
{
    [SerializeField] EnemyHP Hp;
    [SerializeField] Animator anim;
    private void Start()
    {
       if(anim==null) anim = GetComponent<Animator>();
       if(Hp==null) Hp = GetComponent<EnemyHP>();
    }
    private void FixedUpdate()
    {
        if (!Hp.IsAlive) anim.SetBool("IsAlive",Hp.IsAlive);
    }
}
