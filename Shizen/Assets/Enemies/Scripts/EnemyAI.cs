using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float alertedTimer = 5f;

    /// <summary>
    /// Those with highest range will be used first, otherwise first in list has highest priority
    /// </summary>
    [SerializeField] EnemyAttack[] attacks;
    [Tooltip("Make sure to use correct animation transition conditions")]
    private int currentAttackType = 0;
    private bool hasAvailableAtk = false;
    [SerializeField] bool lookAt = true;

    [Header("Hearing")]
    [SerializeField] public float baseHearingRange = 2f;
    [SerializeField] public float hearingAlertedStateIncrease = 8f;
    [SerializeField] float hearingRange = 0f;

    [Header("Vision")]
    [SerializeField] public float baseVisionRange = 10f;
    [SerializeField] public float visionAlertedStateIncrease = 5f;
    [SerializeField] float visionRange = 0f;
    [Range(0, 180)][SerializeField] public float visionAngle = 10f;
    [SerializeField] LayerMask ObstructionLayers;

    private EnemyHP myLife;

    private float distanceToTarg = 0;
    private NavMeshAgent agent;
    private Animator anim;

    private int provokedCount = 0;
    private bool Provoked = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        hearingRange = baseHearingRange;
        visionRange = baseVisionRange;
        myLife = GetComponent<EnemyHP>();
        if (myLife == null) myLife = GetComponentInChildren<EnemyHP>();
        anim = GetComponentInChildren<Animator>();

        //if (target == null) target = FindFirstObjectByType<PlayerHP>().transform;

        StopAgent();

        //attacks.OrderByDescending(x => x.Range);

        //foreach (EnemyAttack attack in attacks)
        //{
        //    attack.FullAmmo();
        //}
    }

    void Update()
    {
        if (myLife != null && myLife.IsAlive)
        {
            distanceToTarg = Vector3.Distance(target.position, transform.position);

            if (distanceToTarg <= hearingRange || HasVisualOnPlayer()) Provoked = true;
            else Provoked = false;

            if (Provoked)
            {
                StartCoroutine(AlertedStateTimer());
                SetAttackType();
                if (distanceToTarg <= agent.stoppingDistance)
                {
                    Attack();
                }
                else if(distanceToTarg <= hearingRange || HasVisualOnPlayer()) ChaseTarget();
            }
            else anim.SetTrigger("Idle");
        } else { Destroy(gameObject); }
    }

    private bool HasVisualOnPlayer()
    {
        Collider[] overlap = Physics.OverlapSphere(transform.position, visionRange);
        if (overlap.Contains(target.gameObject.GetComponentInChildren<Collider>()))
        {
            Vector3 dirToTarg = (target.position - transform.position);

            if (Vector3.Angle(dirToTarg, transform.forward) < visionAngle &&
                !Physics.Raycast(transform.position, dirToTarg, distanceToTarg, ObstructionLayers))
            {
                return true;
            }
        }
        return false;
    }

    private void ChaseTarget()
    {
        if (!anim.GetBool("Attack"))
        {
            hearingRange = baseHearingRange + hearingAlertedStateIncrease;
            visionRange = baseVisionRange + visionAlertedStateIncrease;
            agent.SetDestination(target.position);

            anim.SetTrigger("Move");
        }
        anim.SetBool("Attack", false);
    }

    private void Attack()
    {
        if (hasAvailableAtk)
        {
            if (lookAt)
            {
                var lookat = target.position;
                lookat.y = transform.position.y;
                transform.LookAt(lookat);
            }

            anim.SetBool("Attack", true);
            anim.SetInteger("AttackType", currentAttackType);
        }
        else
        {
            anim.SetBool("Idle", true);
            anim.SetBool("Attack", false);
        }
    }

    /// <summary>
    /// Set an event triger on the animation and put this method in there
    /// </summary>
    public void UseAmmoNoProjectile()
    {
        //var atk = attacks[currentAttackType];
        //if (atk.Ammo > 0 || atk.HasInfiniteAmmo)
        //{
        //    atk.UseAmmo();
        //    StartCoroutine(atk.Reload());
        //    HitboxMeleeAtk(atk);
        //}
    }

    public void UseAmmoWithProjectile()
    {
        //var atk = attacks[currentAttackType];
        //if (atk.Ammo > 0 || atk.HasInfiniteAmmo)
        //{
        //    atk.UseAmmo();
        //    StartCoroutine(atk.Reload());
        //    FireProjectile(atk);
        //}
    }

    public LayerMask AttackMask;
    private void FireProjectile(EnemyAttack atk)
    {
        //if (atk.Projectile != null)
        //{
        //    var obj = Instantiate(atk.Projectile, transform);
        //    obj.transform.SetPositionAndRotation(transform.position, transform.rotation);
        //    obj.transform.SetParent(null);
        //    var projectile = obj.AddComponent<EnemyProjectile>();
        //    projectile.speed = atk.projectileSpeed;
        //    projectile.damage = atk.Damage;
        //    projectile.range = atk.Range;
        //    projectile.target = target.position;
        //    projectile.mask = AttackMask;
        //}
    }

    private void HitboxMeleeAtk(EnemyAttack atk)
    {
        //if (atk.MeleeHitArea != null)
        //{
        //    atk.MeleeHitArea.enabled = true;
        //    var melee = atk.MeleeHitArea.GetComponent<EnemyMeleeAtk>();
        //    if (melee == null)
        //    {
        //        melee = atk.MeleeHitArea.AddComponent<EnemyMeleeAtk>();
        //        melee.damage = atk.Damage;
        //    }
        //}
    }
    public void MeleeHitboxOff(int attackType)
    {
        //var atk = attacks[attackType];
        //if (atk.MeleeHitArea != null)
        //{
        //    atk.MeleeHitArea.enabled = false;
        //    var melee = atk.MeleeHitArea.GetComponent<EnemyMeleeAtk>();
        //    melee.damage = atk.Damage;
        //    melee.ClearHPObjectsHit();
        //}
    }

    private void SetStoppingDistance()
    {
        //agent.stoppingDistance = attacks[currentAttackType].Range;
    }

    /// <summary>
    /// Can be called through animation triggers
    /// </summary>
    public void StartAgent()
    {
        agent.isStopped = false;
    }
    /// <summary>
    /// Can be called through animation triggers
    /// </summary>
    public void StopAgent()
    {
        agent.isStopped = true;
    }

    private void SetAttackType()
    {
        SetStoppingDistance();

        hasAvailableAtk = false;
        //for (int i = 0; i < attacks.Length; i++)
        //{
        //    var atk = attacks[i];
        //    if (atk.Ammo > 0)
        //    {
        //        currentAttackType = atk.AnimationIndex;
        //        hasAvailableAtk = true;
        //        break;
        //    }
        //    if (atk.HasInfiniteAmmo)
        //    {
        //        currentAttackType = atk.AnimationIndex;
        //        hasAvailableAtk = true;
        //    }
        //}
    }

    /// <summary>
    /// leaves range altered for set amount of seconds and then resets to base range
    /// </summary>
    /// <returns></returns>
    private IEnumerator AlertedStateTimer()
    {
        if (provokedCount <= 0)
        {
            provokedCount++;
            yield return new WaitUntil(() => Provoked == false);
            provokedCount--;
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            yield return new WaitForSeconds(alertedTimer);

            if (provokedCount == 0)
            {
                hearingRange = baseHearingRange;
                visionRange = baseVisionRange;
            }
        }
    }
}


