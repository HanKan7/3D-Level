using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    public Animator anim;
    public Animator churchAnim;
    public Animator bustedAnim;

    public IEnumerator BustedAnimationEvent()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }
}
