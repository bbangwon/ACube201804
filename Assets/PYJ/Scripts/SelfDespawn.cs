using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;
using UniRx;

public class SelfDespawn : MonoBehaviour {
    ParticleSystem[] arrParticl;

	private void Awake()
	{
        arrParticl = GetComponentsInChildren<ParticleSystem>();
	}

	// Use this for initialization
	void Start () {
        Observable.IntervalFrame(360)
                  .First()
                  .Subscribe(_ =>
                  {
                      transform.parent = PoolManager.Pools["ParticlePool"].transform;
                      PoolManager.Pools["ParticlePool"].Despawn(this.transform);
                  });
	}
}
