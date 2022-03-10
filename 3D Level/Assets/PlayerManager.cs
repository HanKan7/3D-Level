using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    public bool playerCollectedTheItem = false;
    public Transform resetTransform;
    StarterAssets.ThirdPersonController thirdPersonController;
    private void Update()
    {
        //ResetPosition();
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sea")
        {
            Debug.Log("Fallen into the sea");
            ResetPosition();
        }
        if (other.gameObject.tag == "Church")
        {
            Debug.Log("Collected Item");
            playerCollectedTheItem = true;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Enemy")
        {
            Debug.Log("Collided with enemy");
            ResetPosition();
        }
    }

    public void ResetPosition()
    {
        thirdPersonController = GetComponent<StarterAssets.ThirdPersonController>();
        thirdPersonController.enabled = false;
        transform.position = resetTransform.position;
        StartCoroutine("enableTPC");
    }

    IEnumerator enableTPC()
    {
        yield return new WaitForSeconds(0.2f);
        thirdPersonController.enabled = true;
    }
}
