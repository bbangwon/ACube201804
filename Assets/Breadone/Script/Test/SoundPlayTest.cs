using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Soundable>().PlaySound(SoundInfo.Sounds.MONSTER_DIE);
	}


}
