using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class SoundPlayable : MonoBehaviour {

    AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(string clipName)
    {
        if (audioSource.isPlaying)
            audioSource.Stop();

        audioSource.Play();       
    }

    public void Stop()
    {
        audioSource.Stop();
    }


	
}
