using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    GameObject hero;

    public float moveSpeed = 5f;

    // Use this for initialization
    void Start () {
        hero = GameObject.FindGameObjectWithTag("Player");
	}
	
    private void LateUpdate()
    {
        Vector3 dir = (hero.transform.position - transform.position).normalized; 
        transform.Translate(dir * Time.deltaTime * moveSpeed);
    }

    public void Die()
    {
        Debug.Log("DIe");
        Destroy(gameObject);
    }
    

}
