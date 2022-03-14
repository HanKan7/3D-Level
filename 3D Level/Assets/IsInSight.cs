using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsInSight : MonoBehaviour
{
    public GameObject player;
    public bool isInSight = false;

    private void Start()
    {
        player = GameObject.Find("PlayerArmature");
    }

    private void Update()
    {
        
        if (Vector3.Dot(transform.forward, transform.position - player.transform.position) < 0)
        {
            float angle = Mathf.Acos(Vector3.Dot(transform.forward, transform.position - player.transform.position) /
                (Vector3.Magnitude(transform.forward) * Vector3.Magnitude(transform.position - player.transform.position))) * 180 / Mathf.PI;
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (angle > 143 && distance < 30) 
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), (player.transform.position - new Vector3(0, 0.8f, 0) - transform.position + new Vector3(0, 1, 0)), out hit, distance + 5f))
                {
                    if (hit.collider.gameObject.tag == "Player")
                    {
                        isInSight = true;
                    }
                    else
                    {
                        isInSight = false;
                    }

                }
                else
                {
                    isInSight = false;
                }
                
            } 
            else isInSight = false;
        }
        else
        {
            isInSight = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + new Vector3(0, 1, 0), (player.transform.position - new Vector3(0,0.8f, 0) - transform.position + new Vector3(0, 1, 0)) * 1.2f);
    }
}
