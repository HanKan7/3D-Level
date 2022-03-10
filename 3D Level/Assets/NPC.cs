using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public NavMeshAgent agent;
    Animator anim;
    public List<Transform> wayPoints;
    public Vector3 Destination;
    int i = 0;
    public int random = 0;
    public bool randomIsSet = false;
    public bool positionIsSet = false;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        random = Random.Range(1, 8);
        randomIsSet = true; 
    }

    // Update is called once per frame
    void Update()
    {
        
        RoamingAround();
    }

    private void RoamingAround()
    {
        if(random == 1 && !positionIsSet)
        {
            Destination = new Vector3(Random.Range(436, 454), 6, Random.Range(518, 530));
            agent.SetDestination(Destination);
            positionIsSet = true;

        }
        else if (random == 2 && !positionIsSet)
        {
            Destination = new Vector3(Random.Range(470, 489), 6, Random.Range(522, 525));
            agent.SetDestination(Destination);
            positionIsSet = true;

        }
        else if (random == 3 && !positionIsSet)
        {
            Destination = new Vector3(Random.Range(486, 494), 6, Random.Range(527, 583));
            agent.SetDestination(Destination);
            positionIsSet = true;

        }
        else if (random == 4 && !positionIsSet)
        {
            Destination = new Vector3(Random.Range(505, 516), 6, Random.Range(562, 583));
            agent.SetDestination(Destination);
            positionIsSet = true;

        }
        else if (random == 5 && !positionIsSet)
        {
            Destination = new Vector3(Random.Range(439, 443), 6, Random.Range(560, 586));
            agent.SetDestination(Destination);
            positionIsSet = true;

        }
        else if (random == 6 && !positionIsSet)
        {
            Destination = new Vector3(Random.Range(460, 467), 6, Random.Range(538, 583));
            agent.SetDestination(Destination);
            positionIsSet = true;

        }
        else if (random == 7 && !positionIsSet)
        {
            Destination = new Vector3(Random.Range(505, 526), 6, Random.Range(535, 545));
            agent.SetDestination(Destination);
            positionIsSet = true;

        }
        else if(random == 8 && !positionIsSet)
        {
            i = (i + 1) % (wayPoints.Count);
            Destination = wayPoints[i].transform.position;
            if (Vector3.Distance(Destination, agent.transform.position) > 1f)
            {
                agent.SetDestination(Destination);
            }
            positionIsSet = true;
        }
        if (Vector3.Distance(Destination, agent.transform.position) <= 3f)
        {
            random = Random.Range(1, 8);
            positionIsSet = false;
        }
        //Destination = wayPoints[i].transform.position;
        //if (Vector3.Distance(Destination, agent.transform.position) > 1f)
        //{

        //    agent.SetDestination(Destination);
        //}

        //else
        //{
        //    i = (i + 1) % (wayPoints.Count);
        //}
    }
}
