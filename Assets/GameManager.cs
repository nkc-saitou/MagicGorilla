using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    BaseEnemy bossBase;
    

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
                IsGameClear();
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

    public void IsGameClear()
    {
        Debug.Log("Clear");
    }

    public void IsGameOver()
    {

    }
}
