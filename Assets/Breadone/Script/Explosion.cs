using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Explosion : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DOVirtual.DelayedCall(1f, () =>
        {
            Destroy(gameObject);
        });
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
