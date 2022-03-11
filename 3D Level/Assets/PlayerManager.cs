using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerManager : MonoBehaviour
{
    public bool playerCollectedTheItem = false;
    public Transform resetTransform;
    StarterAssets.ThirdPersonController thirdPersonController;
    SceneTransitions sceneTransitions;
    
    private void Start()
    {
        sceneTransitions = FindObjectOfType<SceneTransitions>();
    }
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
            sceneTransitions.churchAnim.SetTrigger("trigger");
            StartCoroutine("ChasePlayerAfterSomeTime");
        }
        if (playerCollectedTheItem)
        {
            if (other.gameObject.tag == "Goal")
            {
                Debug.Log("Reached Goal");
                sceneTransitions.bustedAnim.GetComponentInChildren<TMPro.TMP_Text>().text = "YOU WIN!";
                sceneTransitions.bustedAnim.SetTrigger("busted");
                sceneTransitions.StartCoroutine(sceneTransitions.BustedAnimationEvent());
            }
        }
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Enemy")
        {
            Debug.Log("Collided with enemy");
            Busted();
        }
    }

    public void ResetPosition()
    {
        thirdPersonController = GetComponent<StarterAssets.ThirdPersonController>();
        thirdPersonController.enabled = false;
        transform.position = resetTransform.position;
        StartCoroutine("enableTPC");
    }

    public void Busted()
    {
        sceneTransitions.bustedAnim.GetComponentInChildren<TMPro.TMP_Text>().text = "BUSTED!";
        sceneTransitions.bustedAnim.SetTrigger("busted");
        StartCoroutine(sceneTransitions.BustedAnimationEvent());
    }

    IEnumerator enableTPC()
    {
        //sceneTransitions.anim.SetTrigger("End");
        yield return new WaitForSeconds(.2f);
        thirdPersonController.enabled = true;
    }

    IEnumerator ChasePlayerAfterSomeTime()
    {
        yield return new WaitForSeconds(4.0f);
        playerCollectedTheItem = true;
    }

}
