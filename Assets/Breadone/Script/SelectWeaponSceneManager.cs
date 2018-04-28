using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SelectWeaponSceneManager : MonoBehaviour {

    public Toggle[] weaponToggles;
    public InputField heroNameIF;

	// Use this for initialization
	void Start () {
       

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

        HeroInfo.Instance.HeroName = heroName;
        HeroInfo.Instance.WeaponIndex = itemIndex;
    }
}
