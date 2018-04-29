using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    static GameManager instance;
    public static GameManager Instance{
        get { return instance; }
    }

    public float minOrthoSize = 2f;
    public float maxOrthoSize = 12f;
    public float timer = 60f;
    public int killCnt = 0;

    public bool isCalcTimer = false;

    public Transform flowers;
    public GameObject[] arrFlower;

	private void Awake()
	{
        instance = this;
        Application.targetFrameRate = 60;
	}

	void Start () {
        Camera.main.orthographicSize = minOrthoSize;

        StartCoroutine(CoTimer());

        float halfHeight = 13f;
        float halfWidth = 13f * (16f / 9f);

        for (int i = 0; i < 150; i++)
        {
            GameObject go = Instantiate(arrFlower[Random.Range(0, arrFlower.Length)], new Vector3(Random.Range(-halfWidth, halfWidth), Random.Range(-halfHeight, halfHeight), 0f), Quaternion.identity, flowers);
            go.transform.localScale = Vector3.one * Random.Range(1.5f, 3f);
        }
    }

    IEnumerator CoTimer(){
        yield return new WaitForSeconds(1f);
        timer += 0.1f;
        while(enabled && gameObject.activeSelf){
            UIManager.Insatnce.UpdateTimer((int)timer);
            isCalcTimer = true;
            yield return new WaitForSeconds(1f);
        }
    }

	
	// Update is called once per frame
	void Update () {
        if(isCalcTimer){
            timer -= Time.deltaTime;
        }
        float ratio = 1 - (timer / 60f);

        if(timer <= 0f)
        {
            HeroInfo.Instance.TotalKills = killCnt;
            HeroInfo.Instance.clearGame = true;
            SceneManager.LoadScene("Ending");
            
        }

        EnemyController.moveSpeed = EnemyController.initMoveSpeed + ((60 - Instance.timer) * 0.01f) + ((Instance.killCnt / 100) * 0.1f);


        //3 - 9
        Camera.main.orthographicSize = minOrthoSize + (maxOrthoSize - minOrthoSize) * ratio;
	}
}
