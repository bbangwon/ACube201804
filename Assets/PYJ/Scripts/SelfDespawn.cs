using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;
using UniRx;

public class SelfDespawn : MonoBehaviour {
    ParticleSystem[] arrParticl;
    System.IDisposable interval;

	private void Awake()
	{
        arrParticl = GetComponentsInChildren<ParticleSystem>();
	}

	// Use this for initialization
	void Start () {
        interval = Observable.IntervalFrame(360)
                  .First()
                  .Subscribe(_ =>
                  {
                      transform.parent = PoolManager.Pools["ParticlePool"].transform;
                      PoolManager.Pools["ParticlePool"].Despawn(this.transform);
                  });
	}

    private void OnDestroy()
    {
        if (interval != null)
            interval.Dispose();
    }
}
