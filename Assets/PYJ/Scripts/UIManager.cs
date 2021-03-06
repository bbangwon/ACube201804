﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    static UIManager instance;
    public static UIManager Insatnce{
        get { return instance; }
    }

    Player player;

    public Text textDebugSpawnCnt;
    public Text textKillCnt;
    public Text textKill;
    public Image panelKill;
    public Text textName;
    public Text textHP;

    public Text textTimer;
    public Image imgTimer;

    public Slider sliderHP;

    public GameObject panelCharacter;

    EnemySpawner spawner;

	private void Awake()
	{
        instance = this;
	}

	private void Start()
	{
        spawner = FindObjectOfType<EnemySpawner>();
        player = FindObjectOfType<Player>();
        textName.text = HeroInfo.Instance.HeroName;
	}

	private void Update()
	{
        textDebugSpawnCnt.text = spawner.spawnCnt.ToString();
        textKill.transform.localScale = Vector3.Lerp(textKillCnt.transform.localScale, Vector3.one, 0.1f);
        panelKill.transform.localScale = Vector3.Lerp(panelKill.transform.localScale, Vector3.one, 0.1f);
        textKillCnt.transform.localScale = Vector3.Lerp(textKill.transform.localScale, Vector3.one, 0.1f);

        imgTimer.fillAmount = GameManager.Instance.timer / 60f;
	}

    Vector3 targetScale = Vector3.zero;
    public void UpdateKillCnt(int killCnt){
        if(killCnt % 30 == 0){
            targetScale = Vector3.one * 3f;
        }
        else{
            targetScale = Vector3.one * 1.5f;
        }
        if (textKillCnt.transform.localScale.x < 1.5f) {
            textKill.transform.localScale = targetScale;
            textKillCnt.transform.localScale = targetScale;

            if(killCnt % 30 == 0){
                panelKill.transform.localScale = Vector3.one * 1.2f;
            }
            else{
                panelKill.transform.localScale = Vector3.one * 1.1f;
            }
        }
        textKillCnt.text = GameManager.Instance.killCnt.ToString();
    }

    public void UpdateHP(float hp){
        textHP.text = hp.ToString();
        sliderHP.value = hp / player.maxHp;
    }

    public void UpdateTimer(int time){
        textTimer.text = time.ToString();
    }
}
