using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;


public class Player : MonoBehaviour {

    public GameObject bulletPrefab;
    public Transform spawnPosObject;

    public GameObject magicParticlePrefab;

    public float force = 5f;

    // Use this for initialization
	void Start () {
        InputManager.Instance.EventSwipe += OnSwipe;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnSwipe(Vector3 dir)
    {
        Debug.Log(dir);
        dir.Normalize();

        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
            
        Bullet bullet = Instantiate(bulletPrefab, spawnPosObject.position, spawnPosObject.rotation).GetComponent<Bullet>();

        bullet.Shoot(dir * force);

        Transform particle = PoolManager.Pools["ParticlePool"].Spawn(magicParticlePrefab.transform, spawnPosObject.position, spawnPosObject.rotation, this.transform);
        particle.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
    }
}
