using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

public class SCR_EnemyAI : MonoBehaviour
{
    public float enemySpeed = 3.5f;
    public float enemyChaseDistance = 5f;
    public float attackRange = 1.5f;

    float timeSinceLastSawThePlayer = Mathf.Infinity;
    float timeSinceArrivedAtWaypoint = Mathf.Infinity;
    protected float timeSinceLastAttack = Mathf.Infinity;


    protected private GameObject player;

    NavMeshAgent navMeshAgent;

    SCR_Health health;
    protected SCR_Health playerHealth;

    [SerializeField] SCR_PatrolPath patrolPath;

    //Patrol Handling Variables
    public float suspicionTime = 5f;
    int currentWaypontIndex = 0;
    private float waypointTolerance = 1f;
    float waypointDwellTime = 3f;
    Vector3 enemyPostiton;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyPostiton = transform.position;
        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<SCR_Health>();
        health = this.gameObject.GetComponent<SCR_Health>();
        //navMeshAgent.speed = enemySpeed;
    }

    void Update()
    {

        if (health.IsDead())
        {
            navMeshAgent.enabled = false;
            return;
        }

        if (InChaseRange())
        {
            ChaseBehaviour();
        }
        else if (timeSinceLastSawThePlayer < suspicionTime)
        {
            SuspiciousBehaviour();
        }
        else
        {
            PatrolBehaviour();
        }

        UpdateTimers();
    }

    private void SuspiciousBehaviour()
    {
        navMeshAgent.isStopped = true;
    }

    private void UpdateTimers()
    {
        timeSinceLastSawThePlayer += Time.deltaTime;
        timeSinceArrivedAtWaypoint += Time.deltaTime;
        timeSinceLastAttack += Time.deltaTime;
    }



    protected virtual void PatrolBehaviour()
    {
        Vector3 nextPosition = enemyPostiton;


        if (patrolPath != null)
        {
            if (AtWaypoint())
            {
                timeSinceArrivedAtWaypoint = 0;
                CycleWayoint();
            }
            nextPosition = GetCurrentWaypoint();
        }

        if (timeSinceArrivedAtWaypoint > waypointDwellTime)
        {
            navMeshAgent.destination = nextPosition;
            navMeshAgent.isStopped = false;
        }

    }

    private bool InAttackRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        return distanceToPlayer < attackRange;
    }

    protected virtual void ChaseBehaviour()
    {
        
        timeSinceLastSawThePlayer = 0f;
        navMeshAgent.isStopped = false;
        transform.LookAt(player.transform.position);
        navMeshAgent.destination = player.transform.position;

        if (InAttackRange())
        {
            navMeshAgent.isStopped = true;
            Attack();
        }
    }

    public bool InChaseRange()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        return distanceToPlayer < enemyChaseDistance;
    }

    protected virtual void Attack()
    {

        //empty
    }

    public void ApplyStasis(float slowMultiplier)
    {
        if (navMeshAgent.speed > 0)
        {
            navMeshAgent.speed *= slowMultiplier;
        }
        return;
    }

    private Vector3 GetCurrentWaypoint()
    {
        return patrolPath.GetWaypoint(currentWaypontIndex);
    }

    private bool AtWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
        return distanceToWaypoint < waypointTolerance;
    }
    private void CycleWayoint()
    {
        currentWaypontIndex = patrolPath.GetNextIndex(currentWaypontIndex);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, enemyChaseDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
