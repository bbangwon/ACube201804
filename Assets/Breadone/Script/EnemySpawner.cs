using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Linq;
using PathologicalGames;

public class EnemySpawner : MonoBehaviour {
    static EnemySpawner instance;
    public static EnemySpawner Instance{
        get { return instance; }
    }

    public Transform[] arrSpawnPos;

    public GameObject[] EnemyPrefabs;

    public int totalWave = 0;
    public int wave = 0;

    public int minSpawnCntPerOnce = 3;
    public int maxSpawnCntPerOnce = 5;

    public int addSpawnCntByKill = 0;

    public int spawnCnt = 0;
    public int maxSpawnCnt = 500;

    public float spawnDelay = 1f;

	private void Awake()
	{
        instance = this;

        arrSpawnPos = new Transform[transform.childCount];
        for (int i = 0; i < arrSpawnPos.Length; i++)
        {
            arrSpawnPos[i] = transform.GetChild(i);
        }
    }

    // Use this for initialization
	void Start () {
        StartCoroutine(SpawnTest());
    }
	
	// Update is called once per frame
	void Update () {
        float size = Camera.main.orthographicSize;
        float halfWidth = size * 1.77f;

        for (int i = 0; i < arrSpawnPos.Length; i++)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, (360f / arrSpawnPos.Length) * i));
            Vector3 targetPos = transform.right * (halfWidth + 5f);
            targetPos.y = targetPos.y * 0.7f;
            arrSpawnPos[i].localPosition = targetPos;
        }
        transform.rotation = Quaternion.identity;
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
                if(wave % 3 == 0){
                    minSpawnCntPerOnce++;
                    maxSpawnCntPerOnce++;
                }

                addSpawnCntByKill = GameManager.Instance.killCnt / 50;
                maxSpawnCnt = Mathf.Min(200 + GameManager.Instance.killCnt / 2, 600);

                EnemySpawn(Random.Range(minSpawnCntPerOnce + addSpawnCntByKill, maxSpawnCntPerOnce + addSpawnCntByKill));
                spawnDelay = 1 - (60 - GameManager.Instance.timer) * 0.01f;
                yield return new WaitForSeconds(spawnDelay);
            }
            yield return null;
        }
    }
    
}
