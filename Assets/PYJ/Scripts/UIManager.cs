using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Text textDebugSpawnCnt;
    public Text textKillCnt;

    EnemySpawner spawner;

	private void Start()
	{
        spawner = FindObjectOfType<EnemySpawner>();
	}

	private void Update()
	{
        textDebugSpawnCnt.text = spawner.spawnCnt.ToString();
	}

    public void UpdateKillCnt(){
        textKillCnt.text = GameManager.Instance.killCnt.ToString();
    }
}
