using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillInfo : MonoBehaviour {

    [HideInInspector]
    public static int[] EnemyKillCount = new int[7];


    static public void ResetValues()
    {
        for(int i=0;i<EnemyKillCount.Length;i++)
        {
            EnemyKillCount[i] = 0;
        }
    }


}
