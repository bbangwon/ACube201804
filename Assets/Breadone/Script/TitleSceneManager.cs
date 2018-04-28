﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ACubeGameJamRankSystem.Instance.getOrCreateGameID("Genocide", "제노사이드", (r) =>
        {
            Debug.Log(r.message);
        });
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnStartGame()
    {
        SceneManager.LoadScene("selectWeapon");
    }
}