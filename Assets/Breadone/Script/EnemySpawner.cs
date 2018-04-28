using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Linq;
using PathologicalGames;

public class EnemySpawner : MonoBehaviour {

    public GameObject[] EnemyPrefabs;

    public int totalWave = 0;
    public int wave = 0;

    public int minSpawnCntPerOnce = 1;
    public int maxSpawnCntPerOnce = 5;

    public int spawnCnt = 0;
    public int maxSpawnCnt = 300;

    public float spawnDelay = 1f;

	// Use this for initialization
	void Start () {

        StartCoroutine(SpawnTest());

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //spawnPoint가 -1일경우 랜덤한 위치에서 적 등장.
    //count 는 적 마리수 
    public void EnemySpawn(int count = 1, int spawnPointIdx = -1) 
    {
        for (int cnt=0;cnt<count;cnt++)
        {
            var spawnPoint = gameObject.Children().OrderBy(_ => Random.value).FirstOrDefault().transform.position;
            var enemyPrefab = EnemyPrefabs[Random.Range(0, EnemyPrefabs.Length)]; //랜덤한 적 하나 선택
            var enemy = PoolManager.Pools["MonsterPool"].Spawn(enemyPrefab);
            spawnPoint += new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
            enemy.position = spawnPoint;
        }
    }

    IEnumerator SpawnTest()
    {
        while(enabled && gameObject.activeSelf){
            if(maxSpawnCnt > spawnCnt)
            {
                totalWave++;
                wave++;
                spawnCnt++;
                if(wave % 3 == 0){
                    minSpawnCntPerOnce++;
                    maxSpawnCntPerOnce++;
                }

                EnemySpawn(Random.Range(minSpawnCntPerOnce, maxSpawnCntPerOnce));
                yield return new WaitForSeconds(spawnDelay);
            }
        }
    }
    
}
