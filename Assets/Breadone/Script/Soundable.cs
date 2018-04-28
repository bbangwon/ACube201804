using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Soundable : MonoBehaviour {

    public void PlaySound(SoundInfo.Sounds sound)
    {
        SoundManager.Instance.Play(this, sound);
    }
}
