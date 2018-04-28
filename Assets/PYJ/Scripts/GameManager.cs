using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    static GameManager instance;
    public static GameManager Instance{
        get { return instance; }
    }

    public float minOrthoSize = 2f;
    public float maxOrthoSize = 9f;
    public float timer = 60f;
    public int killCnt = 0;

    public bool isCalcTimer = false;

	private void Awake()
	{
        instance = this;
	}

	void Start () {
        Camera.main.orthographicSize = minOrthoSize;

        StartCoroutine(CoTimer());
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

        EnemyController.moveSpeed = EnemyController.initMoveSpeed + ((60 - Instance.timer) * 0.01f) + ((Instance.killCnt / 100) * 0.1f);


        //3 - 9
        Camera.main.orthographicSize = minOrthoSize + (maxOrthoSize - minOrthoSize) * ratio;
	}
}
