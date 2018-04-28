using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;
using System;

public class EnemyController : MonoBehaviour {

    public enum ENEMY_TYPE
    {
        DUCK,
        JACK,
        LEGO,
        NUTCRACKER,
        ROBOT,
        SNOWMAN,
        TREE
    }

    public ENEMY_TYPE enemyType;

    GameObject hero;

    public static float initMoveSpeed = 0.8f;
    public static float moveSpeed = 1f;

    public bool isDead = false;

    public bool isTouchPlayer = false;
    public float lifeTime = 3f;

    // Use this for initialization
    void Start () {
        hero = GameObject.FindGameObjectWithTag("Player");
 
	}

    void OnSpawned(){
        EnemySpawner.Instance.spawnCnt++;
        isDead = false;
        isTouchPlayer = false;
        lifeTime = 3f;
        StartCoroutine(MovingAny());
    }

    void OnDespawned(){
        EnemySpawner.Instance.spawnCnt--;
    }

	private void Update()
	{
        if(isTouchPlayer){
            if(lifeTime >= 0f){
                lifeTime -= Time.deltaTime;
            }
            else{
                Attack();
            }
        }
	}

	private void LateUpdate()
    {
        Vector3 dir = (hero.transform.position - transform.position).normalized; 
        transform.Translate(dir * Time.deltaTime * moveSpeed);
    }

    private void OnParticleCollision(GameObject other)
    {
        if(isDead){
            return;
        }

        Magic magic = other.GetComponent<Magic>();
        if (magic)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        GameManager.Instance.killCnt++;
        EnemyKillInfo.EnemyKillCount[(int)enemyType]++;

        UIManager.Insatnce.UpdateKillCnt(GameManager.Instance.killCnt);

        SoundManager.Instance.Play(gameObject, SoundInfo.Sounds.MONSTER_DIE);

        var exp = PoolManager.Pools["ParticlePool"].Spawn("Explosion");
        exp.position = this.transform.position;

        PoolManager.Pools["MonsterPool"].Despawn(this.transform);

    }

    public void Attack(){
        isDead = true;
        SoundManager.Instance.Play(gameObject, SoundInfo.Sounds.MONSTER_DIE);
        PoolManager.Pools["MonsterPool"].Despawn(this.transform);
    }

    IEnumerator MovingAny()
    {
        int toggle = 1;
        while(true)
        {
            toggle *= -1;
            transform.localEulerAngles = Vector3.forward * toggle * 10f;
            yield return new WaitForSeconds(0.1f);
            transform.localEulerAngles = Vector3.zero;
            yield return new WaitForSeconds(0.1f);
        }
        
    }

}
