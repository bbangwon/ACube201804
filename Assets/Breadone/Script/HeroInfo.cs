using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroInfo : Singleton<HeroInfo> {

    [HideInInspector]
    public string HeroName;

    [HideInInspector]
    public int WeaponIndex;

    [HideInInspector]
    public int TotalKills;

    [HideInInspector]
    public int parts_head;

    [HideInInspector]
    public int parts_body;

    [HideInInspector]
    public int parts_leg;

    [HideInInspector]
    public int parts_weapon;

    [HideInInspector]
    public bool clearGame;

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
