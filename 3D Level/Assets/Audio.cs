using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public AudioSource seagulls;
    public AudioSource cityBell;
    public AudioSource alarm;

    public AudioSource waterFall;
    public AudioSource gotItem;
    public AudioSource winSound;
    public AudioSource bustedSound;


    // Start is called before the first frame update
    void Start()
    {
        seagulls.playOnAwake = true;
        cityBell.Play();
        //StartCoroutine(PlayBell());
    }

    IEnumerator PlayBell()
    {
        for (int i = 0; i < 1000; i++)
        {
            yield return new WaitForSeconds(7);
            cityBell.Play();
        }
    }
}
