using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    BaseEnemy bossBase;

    public bool IsStart;
    public bool IsGameClear;
    public bool IsGameOver;
    

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        BossMonitoring();
	}

    /// <summary>
    /// ボスがいたらHP監視
    /// </summary>
    void BossMonitoring()
    {
        if (bossBase)
        {
            if (bossBase.EnemyHP == 0)
            {
                IsGameClear = true;
                GameClear();
            }
        }
        else
        {
            if (GameObject.FindGameObjectWithTag("Boss"))
            {
                bossBase = GameObject.FindGameObjectWithTag("Boss").GetComponent<BaseEnemy>();
            }
        }
    }

    void GameClear()
    {
        Debug.Log("Clear");
    }

    void GameOver()
    {

    }
}
