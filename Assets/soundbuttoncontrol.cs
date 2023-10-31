using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundbuttoncontrol : MonoBehaviour
{
    
    public AudioSource audioSource;
    private bool isAudioPlaying = false;

    private void Start()
    {
        // Find the AudioSource component on the GameObject
        //audioSource = GameObject.Find("AudioManager").GetComponent<AudioSource>();
    }

    public void OnButtonClick()
    {
        if (isAudioPlaying)
        {
            // If audio is playing, stop it and set the flag to false
            audioSource.Stop();
            //isAudioPlaying = false;
           
        }
        else
        {
            // If audio is not playing, start it and set the flag to true
            audioSource.Play();
            //isAudioPlaying = true;
           
        }
        // Toggle the audio state flag
        isAudioPlaying = !isAudioPlaying;
    }
}
