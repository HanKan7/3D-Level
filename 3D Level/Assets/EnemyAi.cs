using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;

public class EnemyAi : MonoBehaviour
{
    bool isTriggerEnemy = false;
    public NavMeshAgent agent;
    Animator anim;

    public Transform player;
    IsInSight isInSight;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;
    public int i = 0;

    //Patroling
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;
    public float wayPointIndex = 0;
    public List<Transform> enemyWayPoints;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange,isChasing = false, wasChasing = false;
    public bool canSeePlayer = false, isInRadius = false, isInSimilarHeight = false, canHearPlayer = false;

    private void Awake()
    {
        player = GameObject.Find("PlayerArmature").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        isInSight = GetComponent<IsInSight>();
    }

    private void Update()
    {
        if (!isTriggerEnemy)
        {
            if (!player.GetComponent<PlayerManager>().playerCollectedTheItem)
            {
                canSeePlayer = isInSight.isInSight;
                isInRadius = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
                isInSimilarHeight = transform.position.y - player.transform.position.y >= -1;
                canHearPlayer = player.GetComponent<StarterAssets.ThirdPersonController>().targetSpeed > 3;

                if (canSeePlayer && isInRadius && isInSimilarHeight && canHearPlayer) playerInSightRange = true;

                else if (!canSeePlayer && isInRadius && isInSimilarHeight && canHearPlayer) playerInSightRange = true;  //Behind enemy
                else if (canSeePlayer && !isInRadius && isInSimilarHeight && canHearPlayer) playerInSightRange = true;   //is outside radius but in front of enemy
                else if (canSeePlayer && isInRadius && !isInSimilarHeight && canHearPlayer) playerInSightRange = false;  // player is above enemy
                else if (canSeePlayer && isInRadius && isInSimilarHeight && !canHearPlayer) playerInSightRange = true; //IFE

                else if (!canSeePlayer && !isInRadius && isInSimilarHeight && canHearPlayer) playerInSightRange = false; //Completely outside vision 
                else if (!canSeePlayer && isInRadius && !isInSimilarHeight && canHearPlayer) playerInSightRange = false;
                else if (!canSeePlayer && isInRadius && isInSimilarHeight && !canHearPlayer) playerInSightRange = false;

                else if (canSeePlayer && !isInRadius && !isInSimilarHeight && canHearPlayer) playerInSightRange = false; //Player too high
                else if (canSeePlayer && !isInRadius && isInSimilarHeight && !canHearPlayer) playerInSightRange = true;

                else if (canSeePlayer && isInRadius && !isInSimilarHeight && !canHearPlayer) playerInSightRange = false;  //same

                else if (!canSeePlayer && isInRadius && isInSimilarHeight && !canHearPlayer) playerInSightRange = false; // player is silently walking behind enemy

                else if (!canSeePlayer && !isInRadius && !isInSimilarHeight && canHearPlayer) playerInSightRange = false;
                else if (!canSeePlayer && isInRadius && !isInSimilarHeight && !canHearPlayer) playerInSightRange = false;  
                else if (canSeePlayer && !isInRadius && !isInSimilarHeight && !canHearPlayer) playerInSightRange = false;
                else if (!canSeePlayer && isInRadius && !isInSimilarHeight && !canHearPlayer) playerInSightRange = false;
                else if (!canSeePlayer && !isInRadius && isInSimilarHeight && !canHearPlayer) playerInSightRange = false;
                else if (!canSeePlayer && !isInRadius && !isInSimilarHeight && !canHearPlayer) playerInSightRange = false;


                if (!playerInSightRange)
                {
                    if (isChasing)
                    {
                        wasChasing = true;
                        isChasing = false;
                        StartCoroutine(ChasePlayerAfterLosingSight(6));
                    }
                    if (!wasChasing)
                    {
                        Patroling();
                    }
                }
                if (playerInSightRange)
                {
                    ChasePlayer();
                    if (Vector3.Distance(transform.position, player.transform.position) < 2f)
                    {
                        player.GetComponent<PlayerManager>().Busted();
                    }   //BUSTED CODE
                }
            }
            else
            {
                ChasePlayer(); // after collecting item
            }
        }
        
    }

    IEnumerator ChasePlayerAfterLosingSight(int seconds)
    {
        anim.SetBool("Run", true);
        agent.speed = 8f;
        agent.SetDestination(player.position);
        yield return new WaitForSeconds(seconds);
        wasChasing = false;
    }

    private void Patroling()
    {
        anim.SetBool("Run", false);
        agent.speed = 3f;
        if (Vector3.Distance(enemyWayPoints[i].transform.position, agent.transform.position) > 1f)
        {
            agent.SetDestination(enemyWayPoints[i].transform.position);
        }
        
        else
        {
            i = (i + 1) % (enemyWayPoints.Count);   
        }


    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 5f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        anim.SetBool("Run", true);
        isChasing = true;
        agent.speed = 12f;
        agent.SetDestination(player.position);  
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);        
    }
}
