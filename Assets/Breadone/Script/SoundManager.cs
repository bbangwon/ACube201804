using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[System.Serializable]
public struct SoundInfo
{
    public enum Sounds
    {
        MONSTER_DIE,
        GAME_CLEAR,
        GAME_FAIL
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

    public void Play(GameObject soundable, SoundInfo.Sounds sound)
    {
        var soundClip = soundInfo.Where(_ => _.whatSound == sound).OrderBy(_ => Random.value).FirstOrDefault();

        if (soundClip.audioClip != default(AudioClip))
        {
            AudioSource.PlayClipAtPoint(soundClip.audioClip, Camera.main.transform.position);
        }
    }

}
