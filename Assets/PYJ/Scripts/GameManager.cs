using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    static GameManager instance;
    public static GameManager Instance{
        get { return instance; }
    }

    public float minOrthoSize = 3;
    public float maxOrthoSize = 9;
    public float timer = 60f;
    public int killCnt = 0;

	private void Awake()
	{
        instance = this;
	}

	void Start () {
        Camera.main.orthographicSize = minOrthoSize;

        StartCoroutine(CoTimer());
	}

    IEnumerator CoTimer(){
        while(enabled && gameObject.activeSelf){
            yield return new WaitForSeconds(1f);

        }
    }

	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        float ratio = 1 - (timer / 60f);

        //3 - 9
        Camera.main.orthographicSize = minOrthoSize + (maxOrthoSize - minOrthoSize) * ratio;
	}
}
