using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AINavMesh : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    [SerializeField]
    Transform movePositionTransform;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        navMeshAgent.destination = movePositionTransform.position;
    }
}
