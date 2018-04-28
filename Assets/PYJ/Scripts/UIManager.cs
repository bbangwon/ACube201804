using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    static UIManager instance;
    public static UIManager Insatnce{
        get { return instance; }
    }

    public Text textDebugSpawnCnt;
    public Text textKillCnt;

    public float killCntPunchDuration = 0f;

    EnemySpawner spawner;

	private void Awake()
	{
        instance = this;
	}

	private void Start()
	{
        spawner = FindObjectOfType<EnemySpawner>();
	}

	private void Update()
	{
        textDebugSpawnCnt.text = spawner.spawnCnt.ToString();
        killCntPunchDuration = 1f;

	}

    public void UpdateKillCnt(){
        textKillCnt.transform.localScale = Vector3.one * 1.2f;
        textKillCnt.text = GameManager.Instance.killCnt.ToString();
    }
}
