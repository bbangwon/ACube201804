using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class ACubeGameJamRankSystem : GJRankSystem {

    const string command_postAvatar = "postAvatar";

    protected override void Awake()
    {
        base.Awake();
        phpFile = "GameJamRank/ACubeGameJamRank.php";
    }

    //아바타 올릴때 호출
    public void postAvatar(string nickname, int head_parts, int body_parts, int leg_parts, int weapon_parts,
        System.Action<resultMessage> result)
    {
        JObject reqContent = new JObject();
        reqContent.Add("nickname", nickname);
        reqContent.Add("gameid", 1);
        reqContent.Add("headparts", head_parts);
        reqContent.Add("bodyparts", body_parts);
        reqContent.Add("legparts", leg_parts);
        reqContent.Add("weaponparts", weapon_parts);

        StartCoroutine(PostData(GetRequestJson(command_postAvatar, reqContent), o => {
            if (result != null)
                result(MakeResult(o));
        }));
    }
}
