using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Linq;
using System.Linq;

public class HeroWeaponChange : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var weapons = gameObject.Descendants().Where(_ => _.name == "Weapon").Children()
            .Select((itm, idx) =>  new { Item = itm, Index = idx });

        var myWeapon = weapons.FirstOrDefault(_ => _.Index == HeroInfo.Instance.WeaponIndex).Item;

        foreach (var w in weapons)
            w.Item.SetActive(false);

        myWeapon.SetActive(true);
    }
}
