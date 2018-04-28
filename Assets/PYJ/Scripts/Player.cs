using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;


public class Player : MonoBehaviour {

    //public GameObject bulletPrefab;
    public Transform spawnPosObject;

    public GameObject magicParticlePrefab;

    public float force = 5f;

    float particleStartSpeed = 1f;
    int particleCycleCnt = 1;
    int particleBurstCnt = 3;
    float particleHalfAngle = 2f;

    public float maxHp = 100f;
    public float hp = 100f;

    public bool isDead = false;

    // Use this for initialization
	void Start () {
        InputManager.Instance.EventSwipe += OnSwipe;

        hp = maxHp;

        StartCoroutine(CoRefreshAbility());
	}

    IEnumerator CoRefreshAbility(){
        while(enabled && gameObject.activeSelf)
        {
            yield return new WaitForSeconds(1f);

            int killCnt = GameManager.Instance.killCnt;
            RefreshAbility(killCnt);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void RefreshAbility(int killCnt) {
        if (killCnt == 0){
            return;
        }

        particleStartSpeed = 1 + (killCnt / 10) * 0.1f;
        particleCycleCnt = Mathf.Min(1 + (killCnt / 250), 3);
        particleBurstCnt = 3 + (killCnt / 10) - ((particleCycleCnt - 1) * 10);
        particleHalfAngle = 2f + (killCnt / 30);
    }

    void OnSwipe(Vector3 dir)
    {
        if(isDead){
            return;
        }

        dir.Normalize();

        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

        Attack(dir);
    }

    void Attack(Vector3 dir){
        Transform particleTr = PoolManager.Pools["ParticlePool"].Spawn(magicParticlePrefab.transform, spawnPosObject.position, spawnPosObject.rotation, this.transform);
        particleTr.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);

        ParticleSystem particle = particleTr.GetComponent<ParticleSystem>();
        var shape = particle.shape;
        shape.angle = particleHalfAngle;
        var emission = particle.emission;
        var burst = emission.GetBurst(0);
        burst.count = particleBurstCnt;
        burst.cycleCount = particleCycleCnt;
        emission.SetBurst(0, burst);
        particle.Play();
    }

	private void OnTriggerEnter2D(Collider2D other)
	{
        if(isDead){
            return;
        }

        EnemyController enemy = other.GetComponent<EnemyController>();
        if(enemy){
            enemy.isTouchPlayer = true;
            hp -= 1f;
            UIManager.Insatnce.UpdateHP(hp);
            if(hp <= 0f){
                Die();
            }
        }
	}

    public void Die(){
        isDead = true;
        Debug.Log("Dead");
    }
}
