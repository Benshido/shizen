using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EnemyAIController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float health;
    private Animator anim;

    [SerializeField] EnemyAttack[] attacks;
    [Tooltip("Make sure to use correct animation transition conditions")]
    private int currentAttackType = 0;
    private bool hasAvailableAtk = false;
    [SerializeField] bool lookAt = true;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    //Waypoints patrol
    public Transform[] waypoints;
    int m_CurrentWaypointIndex;
    //bool wayPointReached = false;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public bool alerted = false;
    private float standardSightRange;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        standardSightRange = sightRange;
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (alerted)
        {
            sightRange = 50;
        }

        if (!playerInSightRange && !playerInAttackRange)
        {
            if (alerted)
            {
                StartCoroutine(EnemyAggro(5));
            }
            agent.speed = 1.5f;
            Patrolling();
        }
        if (playerInSightRange && !playerInAttackRange)
        {
            if (!alerted) CallGroup();
            agent.speed = 3.5f;
            ChasePlayer();
        }
        if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
        }
    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();
        else agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f /*&& !wayPointReached*/)
        {
            StartCoroutine(PatrollingPause(3));
        }
    }

    private void SearchWalkPoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        walkPoint = waypoints[m_CurrentWaypointIndex].position;

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            anim.SetTrigger("Walk");
            walkPointSet = true;
        }
    }

    public void ChasePlayer()
    {
        if (!anim.GetBool("Attack"))
        {
            agent.SetDestination(player.position);
            anim.SetTrigger("Run");
        }
        anim.SetBool("Attack", false);
    }

    public void LastPlayerLocation()
    {
        agent.SetDestination(player.position);
    }

    public void CallGroup()
    {
        // Get the parent of the parent of the parent of the child object
        GameObject grandParentObject = gameObject.transform.parent.parent.gameObject;
        //Debug.Log(gameObject.transform.parent.parent.gameObject.name);
        grandParentObject.GetComponent<AlertGroup>().Alert();
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        anim.SetBool("Attack", true);
        //anim.SetInteger("AttackType", currentAttackType);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    IEnumerator EnemyAggro(float time)
    {
        yield return new WaitForSeconds(time);
        if (!playerInSightRange && !playerInAttackRange)
        {
            sightRange = standardSightRange;
            alerted = false;
            StopAllCoroutines();
        }
    }

    IEnumerator PatrollingPause(float time)
    {
        anim.SetTrigger("Idle");
        yield return new WaitForSeconds(time);
        //wayPointReached = false;
        walkPointSet = false;
        anim.SetTrigger("Walk");
        StopAllCoroutines();
    }
}
