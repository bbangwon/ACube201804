using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathologicalGames;

public class EnemyController : MonoBehaviour {

    GameObject hero;

    public float moveSpeed = 5f;

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
        Debug.Log("DIe");
        PoolManager.Pools["MonsterPool"].Despawn(this.transform);
    }
    

}
