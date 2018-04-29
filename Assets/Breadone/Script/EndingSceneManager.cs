using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingSceneManager : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
        if (PlayerPrefs.GetString("Mod") == "NightMare")
        {
            HeroInfo.Instance.TotalKills = (int)((float)HeroInfo.Instance.TotalKills * 1.1f);
        }
        gameObject.Child("Image").Child("KillCount").GetComponent<Text>().text = HeroInfo.Instance.TotalKills.ToString();
        gameObject.Child("Name").GetComponent<Text>().text = HeroInfo.Instance.HeroName;

        if(HeroInfo.Instance.clearGame)
        {
            SoundManager.Instance.Play(gameObject, SoundInfo.Sounds.GAME_CLEAR);
        }
        else
        {
            SoundManager.Instance.Play(gameObject, SoundInfo.Sounds.GAME_FAIL);
        }

        MakeHero();

        if (PlayerPrefs.GetString("Mod") == "Hard" || PlayerPrefs.GetString("Mod") == "NightMare")
        {            
            ACubeGameJamRankSystem.Instance.postScore(HeroInfo.Instance.HeroName, HeroInfo.Instance.TotalKills, (r) =>
            {
                Debug.Log(r.message);
            });
        }
	}
	
    void MakeHero()
    {
        //히어로 만들고



        //Head.. 파츠변경




        //이미지 파일만들고

        //서버로 전송


    }

    public void OnRetry()
    {
        HeroInfo.Instance.TotalKills = 0;
        HeroInfo.Instance.HeroName = "";

        SceneManager.LoadScene("title");
    }
}
