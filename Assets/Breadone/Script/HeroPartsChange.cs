using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Unity.Linq;
using System.Linq;
using UnityEngine.SceneManagement;

public class HeroPartsChange : MonoBehaviour {

    public Sprite[] partsSprites;

    SpriteRenderer headR;
    SpriteRenderer bodyR;
    SpriteRenderer armR;
    SpriteRenderer legR;

    // Use this for initialization
    void Start () {

        headR = gameObject.Child("head").GetComponent<SpriteRenderer>();
        bodyR = gameObject.Child("body").GetComponent<SpriteRenderer>();
        armR = gameObject.Child("body").Child("armRot").Child("arm").GetComponent<SpriteRenderer>();
        legR = gameObject.Child("leg").GetComponent<SpriteRenderer>();

        if (SceneManager.GetActiveScene().name == "main")
        {
            bool time40 = false;
            bool time20 = false;
            bool time0 = false;
            gameObject.ObserveEveryValueChanged(_ => GameManager.Instance.timer).Where(_ => (int)_ == 40)
                .Subscribe(_ =>
                {
                    if(!time40)
                    {
                        var maxKillEnemyType = EnemyKillInfo.EnemyKillCount.ToList()
                            .Select((itm, idx) => new { Item = itm, Index = idx }).OrderByDescending(k => k.Item)
                            .FirstOrDefault().Index;

                        string prefix = GetSpritePrefix((EnemyController.ENEMY_TYPE)maxKillEnemyType);
                        string find_sprite_name = prefix + "_head";

                        headR.sprite = partsSprites.ToList().FirstOrDefault(p => p.name == find_sprite_name);
                        HeroInfo.Instance.parts_head = maxKillEnemyType;
                        EnemyKillInfo.ResetValues();
                        time40 = true;
                    }


                });

            gameObject.ObserveEveryValueChanged(_ => GameManager.Instance.timer).Where(_ => (int)_ == 20)
                .Subscribe(_ =>
                {
                    if (!time20)
                    {
                        var maxKillEnemyType = EnemyKillInfo.EnemyKillCount.ToList()
                            .Select((itm, idx) => new { Item = itm, Index = idx }).OrderByDescending(k => k.Item)
                            .FirstOrDefault().Index;

                        string prefix = GetSpritePrefix((EnemyController.ENEMY_TYPE)maxKillEnemyType);
                        string find_sprite_name = prefix + "_body";

                        bodyR.sprite = partsSprites.ToList().FirstOrDefault(p => p.name == find_sprite_name);

                        find_sprite_name = prefix + "_arm";
                        armR.sprite = partsSprites.ToList().FirstOrDefault(p => p.name == find_sprite_name);

                        HeroInfo.Instance.parts_body = maxKillEnemyType;
                        EnemyKillInfo.ResetValues();
                        time20 = true;
                    }

                });

            gameObject.ObserveEveryValueChanged(_ => GameManager.Instance.timer).Where(_ => (int)_ == 0)
                .Subscribe(_ =>
                {
                    if (!time0)
                    {
                        var maxKillEnemyType = EnemyKillInfo.EnemyKillCount.ToList()
                            .Select((itm, idx) => new { Item = itm, Index = idx }).OrderByDescending(k => k.Item)
                            .FirstOrDefault().Index;

                        string prefix = GetSpritePrefix((EnemyController.ENEMY_TYPE)maxKillEnemyType);
                        string find_sprite_name = prefix + "_leg";

                        legR.sprite = partsSprites.ToList().FirstOrDefault(p => p.name == find_sprite_name);

                        HeroInfo.Instance.parts_leg = maxKillEnemyType;
                        EnemyKillInfo.ResetValues();
                        time0 = true;
                    }
                });
        }
        else if(SceneManager.GetActiveScene().name == "Ending")
        {
            string prefix = GetSpritePrefix((EnemyController.ENEMY_TYPE)HeroInfo.Instance.parts_head);
            string find_sprite_name = prefix + "_head";

            headR.sprite = partsSprites.ToList().FirstOrDefault(p => p.name == find_sprite_name);

            prefix = GetSpritePrefix((EnemyController.ENEMY_TYPE)HeroInfo.Instance.parts_body);
            find_sprite_name = prefix + "_body";

            bodyR.sprite = partsSprites.ToList().FirstOrDefault(p => p.name == find_sprite_name);

            prefix = GetSpritePrefix((EnemyController.ENEMY_TYPE)HeroInfo.Instance.parts_body);
            find_sprite_name = prefix + "_arm";

            armR.sprite = partsSprites.ToList().FirstOrDefault(p => p.name == find_sprite_name);

            prefix = GetSpritePrefix((EnemyController.ENEMY_TYPE)HeroInfo.Instance.parts_leg);
            find_sprite_name = prefix + "_leg";

            legR.sprite = partsSprites.ToList().FirstOrDefault(p => p.name == find_sprite_name);
        }
    }

    string GetSpritePrefix(EnemyController.ENEMY_TYPE type)
    {
        switch ((EnemyController.ENEMY_TYPE)type)
        {
            case EnemyController.ENEMY_TYPE.DUCK:
                return "duck";
            case EnemyController.ENEMY_TYPE.JACK:
                return "jack";
            case EnemyController.ENEMY_TYPE.LEGO:
                return "lego";
            case EnemyController.ENEMY_TYPE.NUTCRACKER:
                return "nutcracker";
            case EnemyController.ENEMY_TYPE.ROBOT:
                return "robot";
            case EnemyController.ENEMY_TYPE.SNOWMAN:
                return "snowman";
            case EnemyController.ENEMY_TYPE.TREE:
                return "tree";
        }

        return "";
    }
}
