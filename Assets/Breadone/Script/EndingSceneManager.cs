using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Linq;
using UnityEngine.UI;

public class EndingSceneManager : MonoBehaviour {

    
	// Use this for initialization
	void Start () {
        gameObject.Child("KillCount").GetComponent<Text>().text = HeroInfo.Instance.TotalKills.ToString();
        gameObject.Child("Name").GetComponent<Text>().text = HeroInfo.Instance.HeroName;

        MakeHero();


        ACubeGameJamRankSystem.Instance.postScore(HeroInfo.Instance.HeroName, HeroInfo.Instance.TotalKills, (r) =>
        {
            Debug.Log(r.message);
        });

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void MakeHero()
    {
        //히어로 만들고



        //Head.. 파츠변경




        //이미지 파일만들고

        //서버로 전송


    }
}
