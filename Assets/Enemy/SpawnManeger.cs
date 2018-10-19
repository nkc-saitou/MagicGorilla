using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManeger : MonoBehaviour {
    [SerializeField, Header("最大沸き数")]
    private int maxSpawn;
    public int NowSpawn { get; set; }       //今沸いている数
    [SerializeField, Header("沸き間隔")]
    private float waitTime;
    [SerializeField]
    private Vector3 spawnPosA;      //AからBまでの範囲でわく
    [SerializeField]
    private Vector3 spawnPosB;
    [SerializeField]
    private GameObject[] monster; //敵

    private const float rayFall = 30;  //rayを落とす高さ
    private bool startSpawn = false;        //沸き開始
    public bool endSpawn{get;set;}          //沸き終了


    void Start () {
        startSpawn = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (startSpawn)
        {
            startSpawn = false;
            StartCoroutine(LoopSpawner());
        }

    }

    IEnumerator LoopSpawner()
    {
        while (!endSpawn)
        {
            if (NowSpawn < maxSpawn)
            {
                RandomSpawn();
            }
            yield return new WaitForSeconds(waitTime);
        }
    }

    /// <summary>
    /// ランダムの座標にランダムのモンスターを沸かせる
    /// </summary>
    void RandomSpawn()
    {
        int monNum = Random.Range(0, monster.Length);   //モンスターの選択
        Vector3 spawnPos;
        int loop =0;
        while (true)//最大15回実行し駄目だったら終了
        { 
            if (GetRayHitPos(out spawnPos))
            {
                GameObject monsterObj = (GameObject)Instantiate(monster[monNum], spawnPos, Quaternion.identity); //沸かせる
                monsterObj.GetComponent<BaseEnemy>().SpawnManegerCompornent = gameObject.GetComponent<SpawnManeger>();
                NowSpawn++;
                break;
            }
            else
            {
                loop++;
                if (loop > 15)
                {
                    Debug.Log("none");
                    break;
                }
            }
        }
    }

    /// <summary>
    /// ランダムにRayを落として座標を取る
    /// </summary>
    /// <param name="spawnPos">返す座標(沸き位置)</param>
    /// <returns></returns>
    bool GetRayHitPos(out Vector3 spawnPos)
    {
        float posX = Random.Range(spawnPosA.x, spawnPosB.x);//X座標
        float posZ = Random.Range(spawnPosA.z, spawnPosB.z);//Z座標
        Ray ray = new Ray(new Vector3(posX, rayFall, posZ), new Vector3(posX, -10, posZ) - new Vector3(posX,rayFall,posZ));
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Untagged")
            {
                spawnPos = hit.point;
                return true;
            }
        }
        spawnPos = Vector3.zero;
        return false;
    }
}
