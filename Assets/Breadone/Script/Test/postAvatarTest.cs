using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class postAvatarTest : MonoBehaviour {

    // Use this for initialization
    void Start () {
        ((ACubeGameJamRankSystem)ACubeGameJamRankSystem.Instance).postAvatar("빵원", 1, 1, 1, 1, (r) => {
            Debug.Log(r.message);
        });

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
