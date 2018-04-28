using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroInfo : Singleton<HeroInfo> {

    public enum PARTS_INDEX
    {
        DUCK,
        JACK,
        LEGO,
        NUTCRACKER,
        ROBOT,
        SNOWMAN,
        TREE
    }

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
