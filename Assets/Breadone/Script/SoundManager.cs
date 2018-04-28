using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[System.Serializable]
public struct SoundInfo
{
    public enum Sounds
    {
        MONSTER_DIE
    }

    public Sounds whatSound;
    public AudioClip audioClip;
}

public class SoundManager : Singleton<SoundManager> {


    [SerializeField]
    List<SoundInfo> soundInfo;    
    private static SoundManager DontInstance;

    protected override void Awake()
    {
        DontDestroyOnLoad(this);

        if (DontInstance == null)
        {
            DontInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        base.Awake();
    }

    public void Play(Soundable soundable, SoundInfo.Sounds sound)
    {
        var soundClip = soundInfo.Where(_ => _.whatSound == sound).OrderBy(_ => Random.value).FirstOrDefault();

        if(soundClip.audioClip != default(AudioClip))
            soundable.GetComponent<AudioSource>().PlayOneShot(soundClip.audioClip);
    }

}
