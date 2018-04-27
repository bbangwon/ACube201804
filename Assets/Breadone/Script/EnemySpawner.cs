using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Linq;

public class EnemySpawner : MonoBehaviour {

    public GameObject[] EnemyPrefabs;

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
        var spawnPoint = gameObject.Children().OrderBy(_ => Random.value).FirstOrDefault().transform.position;
        for (int cnt=0;cnt<count;cnt++)
        {
            var enemyPrefab = EnemyPrefabs[Random.Range(0, EnemyPrefabs.Length)]; //랜덤한 적 하나 선택
            var enemy = Instantiate<GameObject>(enemyPrefab);            
            spawnPoint += new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0);
            enemy.transform.position = spawnPoint;
        }
    }

    IEnumerator SpawnTest()
    {
        for(int i=0;i<100;i++)
        {
            EnemySpawn(Random.Range(1,10));
            yield return new WaitForSeconds(1f);
        }
    }
    
}
