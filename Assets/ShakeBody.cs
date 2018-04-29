using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeBody : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}

    public void OnSpawned(){
        StartCoroutine(MovingAny());
    }

    public void OnDespawned(){
        StopAllCoroutines();
    }
	
    IEnumerator MovingAny()
    {
        int toggle = 1;
        while (true)
        {
            toggle *= -1;
            transform.localEulerAngles = Vector3.forward * toggle * 8f;
            yield return new WaitForSeconds(0.1f);
            transform.localEulerAngles = Vector3.zero;
            yield return new WaitForSeconds(0.1f);
        }

    }
}
