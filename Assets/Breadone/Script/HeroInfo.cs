using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroInfo : Singleton<HeroInfo> {

    [HideInInspector]
    public string HeroName;

    [HideInInspector]
    public int WeaponIndex;

    protected override void Awake()
    {
        DontDestroyOnLoad(this);

        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        base.Awake();
    }
}
