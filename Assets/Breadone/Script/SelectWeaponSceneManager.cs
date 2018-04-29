using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class SelectWeaponSceneManager : MonoBehaviour {

    public Toggle[] weaponToggles;
    public InputField heroNameIF;

	// Use this for initialization
	void Start () {
        HeroInfo.Instance.parts_head = (int)EnemyController.ENEMY_TYPE.LEGO;
        HeroInfo.Instance.parts_body = (int)EnemyController.ENEMY_TYPE.LEGO;
        HeroInfo.Instance.parts_leg = (int)EnemyController.ENEMY_TYPE.LEGO;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnOK()
    {
        var itemIndex = weaponToggles.Select((item, index) => new
        {
            Index = index,
            Item = item
        }).FirstOrDefault(_ => _.Item.isOn).Index;

        var heroName = heroNameIF.text;
        if(heroName.Length == 0)
        {
            Debug.Log("이름을 적어주세요!");
            return;
        }


        HeroInfo.Instance.HeroName = heroName;
        HeroInfo.Instance.WeaponIndex = itemIndex;

        if(PlayerPrefs.GetString("Mod") == "Hard" || PlayerPrefs.GetString("Mod") == "NightMare"){
            ACubeGameJamRankSystem.Instance.getOrCreateNickname(heroName, (r) =>
            {
                if (r.result)
                {
                    SceneManager.LoadScene("main");
                }
                else
                {
                    Debug.Log(r.message);
                }
            });
        }
        else{
            SceneManager.LoadScene("main");
        }

    }
}
