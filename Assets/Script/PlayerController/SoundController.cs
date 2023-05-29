using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController instance;
    public AudioSource FootStepSound;
    public AudioSource FireSound;
    public AudioSource PickUPSound;

    public AudioSource StartAttack;

    public AudioSource Retreat;


    private void Awake()
    {
        if(instance==null)
        {
            instance = this;

        }
    }
    void Start()
    {
        
    }
    public void FireSoundPlay()
    {
        FireSound.Play();
    }
    
    public void PickUpSoundPlay()
    {
        PickUPSound.Play();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Horizontal")!=0 || Input.GetAxis("Vertical") != 0)
        {
            if (FootStepSound.isPlaying == false)
                   FootStepSound.Play();
        }
       
    }
}
