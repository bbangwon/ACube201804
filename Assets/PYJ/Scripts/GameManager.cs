using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public float minOrthoSize = 3;
    public float maxOrthoSize = 9;
    public float timer = 60f;

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
