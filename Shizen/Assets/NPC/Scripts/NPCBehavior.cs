using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCBehavior : MonoBehaviour
{
    public NavMeshAgent agent;
    public LayerMask whatIsGround;
    private Animator anim;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    bool isWalking = false;

    //0=Idle, 1=Walking, 2=Dying, 3=Dancing, 4=Pointing, 5=Terrified, 6=Praying, 7=Sitting, 8=Talking
    public bool isDead = false;
    public int npcState = 0;

    private bool corountineCalled = false;

    // Start is called before the first frame update
    void Start()
    {
        if(!isDead) agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        switch (npcState)
        {
            case 1:
                anim.SetTrigger("Walking");
                isWalking = true;
                break;
            case 2:
                anim.SetTrigger("Dying");
                break;
            case 3:
                anim.SetTrigger("Dancing");
                break;
            case 4:
                anim.SetTrigger("Pointing");
                break;
            case 5:
                anim.SetTrigger("Terrified");
                break;
            case 6:
                anim.SetTrigger("Praying");
                break;
            case 7:
                anim.SetTrigger("Sitting");
                break;
            case 8:
                anim.SetTrigger("Talking");
                break;
            default:
                anim.SetTrigger("Idle");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead && isWalking)
        {
            Patrolling();

            if (corountineCalled) anim.SetTrigger("Idle");
            else anim.SetTrigger("Walking");
        }
    }

    private void Patrolling()
    {
        if (!walkPointSet) SearchWalkPoint();
        else agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walkpoint reached
        if (distanceToWalkPoint.magnitude < 0.5f)
        {
            StartCoroutine(PatrollingPause(3));
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        //m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        //walkPoint = waypoints[m_CurrentWaypointIndex].position;

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            anim.SetTrigger("Walking");
            walkPointSet = true;
        }
    }

    IEnumerator PatrollingPause(float time)
    {
        if (!corountineCalled)
        {
            corountineCalled = true;
            yield return new WaitForSeconds(time);
            walkPointSet = false;
            corountineCalled = false;
            //StopAllCoroutines();
        }
    }
}
