using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    bool isTriggerEnemy = false;
    public NavMeshAgent agent;
    Animator anim;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;
    public List<Transform> rayTransforms = new List<Transform>();
    public List<Ray> rays = new List<Ray>();

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
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("PlayerArmature").transform;
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        for(int i = 0; i < rayTransforms.Count; i++)
        {
            Ray ray = new Ray(transform.position + new Vector3(0, 1f, 0), rayTransforms[i].transform.position - transform.position);
            rays.Add(ray);
        }
    }

    private void Update()
    {
        if (!isTriggerEnemy)
        {
            if (!player.GetComponent<PlayerManager>().playerCollectedTheItem)
            {
                //Check for sight and attack range
                playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
                playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

                if (!playerInSightRange && !playerInAttackRange) Patroling();
                if (playerInSightRange && !playerInAttackRange)
                {
                    ChasePlayer();
                    if (Vector3.Distance(transform.position, player.transform.position) < 2f)
                    {
                        //player.GetComponent<PlayerManager>().ResetPosition();
                        player.GetComponent<PlayerManager>().Busted();
                    }
                }

                if (playerInAttackRange && playerInSightRange) AttackPlayer();
            }
            else
            {
                ChasePlayer();
            }
        }
        
    }

    private void Patroling()
    {
        //if (!walkPointSet) SearchWalkPoint();

        //if (walkPointSet)
        //    agent.SetDestination(walkPoint);

        //Vector3 distanceToWalkPoint = transform.position - walkPoint;

        ////Walkpoint reached
        //if (distanceToWalkPoint.magnitude < 1f)
        //    walkPointSet = false;
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
        agent.speed = 11f;
        agent.SetDestination(player.position);
    }



    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            //Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            //rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            //rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.blue;
        for(int i = 0; i < rayTransforms.Count; i++)
        {
            Gizmos.DrawLine(transform.position + new Vector3(0, 1f, 0), rayTransforms[i].transform.position);
            //Gizmos.DrawRay(rays[i]);
        }
        
    }
}
