using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SpawnManeger : MonoBehaviour {
    [SerializeField, Header("沸き数"),Tooltip("同時に存在可能な数")]
    private int maxSpawn;
    [SerializeField, Tooltip("同時に沸く最大数")]
    private int maxSameTimeSpawn;
    [SerializeField, Tooltip("ボスが沸くまでの敵の数")]
    private int numberOfSpawnBoss;
    [SerializeField, Tooltip("沸き間隔(秒)")]
    private float waitTime;
    [SerializeField,Header("沸く範囲")]
    private Vector3 spawnPosA;                                      //AからBまでの範囲でわく
    [SerializeField]
    private Vector3 spawnPosB;
    [SerializeField, Tooltip("ボスの沸き位置")]
    private Vector3 spawnPosOfBoss;
    [Space,SerializeField]
    private GameObject[] monster; //敵
    [SerializeField]
    private GameObject bossMonster;//ボス



    const float rayFall = 30;                                       //rayを落とす高さ
    int spawnedMonster;                                             //沸いた合計数

    bool endSpawn;                                                  //沸き終了
    List<BaseEnemy> enemies = new List<BaseEnemy>();                //enemy監視用
    GameManager gManager;


    public int NowSpawn { get; set; }                               //今沸いている数
    public int DestroyedMonster { get; set; }                       //倒した数
    public int BossLimit { get; set; }                              //ボスの沸く数





    void Start () {
        gManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        this.ObserveEveryValueChanged(_ => gManager.IsStart).
            Where(_ => gManager.IsStart).
            Take(1).
            Subscribe(_ => StartCoroutine(LoopSpawner()));

        this.ObserveEveryValueChanged(_ => gManager.IsGameClear).
            Where(_ => _).
            Take(1).
            Subscribe(_ => endSpawn=true);

        this.ObserveEveryValueChanged(_ => spawnedMonster).
            Where(_ => _ >= numberOfSpawnBoss).
            Take(1).
            Subscribe(_ => endSpawn = true);

        this.ObserveEveryValueChanged(_ => DestroyedMonster).
            Where(_ => _ == numberOfSpawnBoss).
            Take(1).
            Subscribe(_ =>
            {
                BossSpawn();
            });

        this.UpdateAsObservable().
            TakeUntilDestroy(this).
            Where(_ => !gManager.IsGameClear && !gManager.IsGameOver).
            Subscribe(_=>EnemyMonitoring());
	}
	



    IEnumerator LoopSpawner()
    {
        while (!endSpawn)
        {
            //敵が最大まで沸いているなら待機する
            while (NowSpawn >= maxSpawn)
            {
                yield return null;
            }
            Debug.Log(endSpawn);
            if (endSpawn)
            {
                break;
            }
            //敵が限界まで沸いていないなら沸かす
            if (NowSpawn < maxSpawn)
            {
                RandomSpawn();
            }
            yield return new WaitForSeconds(waitTime);
        }
    }



    /// <summary>
    /// ボスを沸かせるやつ(β)
    /// </summary>
    void BossSpawn()
    {
        Instantiate(bossMonster,spawnPosOfBoss,Quaternion.identity);
    }



    /// <summary>
    /// Enemyの死亡監視
    /// </summary>
    void EnemyMonitoring()
    {
        foreach(var enemy in enemies)
        {
            if (!enemy)
            {
                NowSpawn--;
                DestroyedMonster++;
                Debug.Log(DestroyedMonster);
            }
        }
        enemies.RemoveAll(enemies => enemies == null);
    }



    /// <summary>
    /// 範囲内でランダムの座標にランダムのモンスターをランダムな数沸かせる(α)
    /// </summary>
    void RandomSpawn()
    {
        int monNum = Random.Range(0, monster.Length);   //モンスターの選択
        int spawnNum = RandomSpawnNumber();             //沸かせる数
        Vector3[] spawnPos=new Vector3[spawnNum];       //沸かせる位置
        for (int i = 0; i < spawnNum; i++)
        {
            for(int j = 0; j < 15; j++)
            {
                if (GetRayHitPos(out spawnPos[i]))
                {
                    bool flg = false;
                    for(int k = 0; k < i; k++)
                    {
                        if (Vector3.Distance(spawnPos[k], spawnPos[i]) < 1)
                        {
                            flg = true;
                            break;
                        }
                    }
                    if (!flg)
                    {
                        GameObject monsterObj = (GameObject)Instantiate(monster[monNum], spawnPos[i], Quaternion.identity); //沸かせる
                        enemies.Add(monsterObj.GetComponent<BaseEnemy>());
                        NowSpawn++;         //生存している敵の数にプラス
                        spawnedMonster++;   //沸かせた数にプラス
                        break;
                    }
                }
            }
        }
    }



    /// <summary>
    /// 規定値を超えないようにランダムな値をとる
    /// </summary>
    int RandomSpawnNumber()
    {
        //同時に沸ける数以下の値でランダム
        int num= Random.Range(1, maxSameTimeSpawn+1);
        //ボスまでの沸かせられる数を超えるなら残りの数に、超えないならそのまま
        num =spawnedMonster+num>numberOfSpawnBoss? numberOfSpawnBoss - spawnedMonster:num;
        //同時に存在できる数を超えるなら存在可能数まで減らす、超えないならそのまま
        num =NowSpawn + num > maxSpawn ? maxSpawn - NowSpawn : num;
        return num;
    }



    /// <summary>
    /// ランダムにRayを落として座標を取る
    /// </summary>
    /// <param name="spawnPos">返す座標(沸き位置)</param>
    /// <returns></returns>
    bool GetRayHitPos(out Vector3 spawnPos)
    {
        float posX = Random.Range(spawnPosA.x, spawnPosB.x);//X座標範囲ランダム
        float posZ = Random.Range(spawnPosA.z, spawnPosB.z);//Z座標範囲ランダム
        int layerMask = ~(1 << 9);//レイヤーマスク

        Ray ray = new Ray(new Vector3(posX, rayFall, posZ), new Vector3(posX, -10, posZ) - new Vector3(posX,rayFall,posZ));
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit,100,layerMask))
        {
            if (hit.collider.tag == "Untagged")
            {
                spawnPos = hit.point+new Vector3(0,0.5f,0);
                return true;
            }
        }
        spawnPos = Vector3.zero;
        return false;
    }
}
