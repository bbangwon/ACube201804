using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;
using System;

public class EnemyController : MonoBehaviour {
    GameObject hero;

    public static float initMoveSpeed = 0.8f;
    public static float moveSpeed = 1f;

    public bool isDead = false;

    // Use this for initialization
    void Start () {
        hero = GameObject.FindGameObjectWithTag("Player");
	}

    void OnSpawned(){
        isDead = false;
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
        UIManager.Insatnce.UpdateKillCnt(GameManager.Instance.killCnt);

        SoundManager.Instance.Play(gameObject, SoundInfo.Sounds.MONSTER_DIE);
        PoolManager.Pools["MonsterPool"].Despawn(this.transform);
    }
    

}
