using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour {

    public GameObject creditui;

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
        PlayerPrefs.SetFloat("hp", 50f);
        SceneManager.LoadScene("selectWeapon");
    }

    public void OnStartEasyGame(){
        PlayerPrefs.SetFloat("hp", 100f);
        PlayerPrefs.SetString("Mod", "Easy");
        SceneManager.LoadScene("selectWeapon");
    }

    public void OnStartHardGame(){
        PlayerPrefs.SetFloat("hp", 10f);
        PlayerPrefs.SetString("Mod", "Hard");
        SceneManager.LoadScene("selectWeapon");
    }

    public void OnStartNightMareGame(){
        PlayerPrefs.SetFloat("hp", 3f);
        PlayerPrefs.SetString("Mod", "NightMare");
        SceneManager.LoadScene("selectWeapon");
    }

     public void OpenCreditUI()
    {
        creditui.SetActive(true);
    }

    public void CloseCreditUI()
    {
        creditui.SetActive(false);
    }
}
