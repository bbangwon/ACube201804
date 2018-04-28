using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using PathologicalGames;

public class Explosion : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DOVirtual.DelayedCall(1f, () =>
        {
            PoolManager.Pools["ParticlePool"].Despawn(transform);
        });
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
