using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class BaseEnemy : MonoBehaviour {
    //プレイヤーの座標
    protected Transform PlayerPos;//{ get; set; }
    //ナビメッシュ
    protected NavMeshAgent agent;

    //移動用パス
    protected NavMeshPath path;
    //体力
    [SerializeField]
    protected float enemyHp=1;

    //スコア
    [SerializeField]
    protected int score=10;


    public float EnemyHP
    {
        get { return enemyHp; }
        set {
            enemyHp = value >= 0 ? value : 0;   //HPがマイナスにならないように
            if (enemyHp == 0)
            {
                Dead();
            }
        }
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (!PlayerPos)//プレイヤーを探す
        {
            PlayerPos = GameObject.FindGameObjectWithTag("Player").transform;
        }
        OnStart();
    }

    /// <summary>
    /// 死亡処理（仮）
    /// </summary>
    protected virtual void Dead()
    {
        GameObject.Find("ScoreManager").GetComponent<ScoreManager>().AddScore(score);
        Destroy(gameObject,0.001f);
    }

    /// <summary>
    /// Startと同義
    /// </summary>
    protected virtual void OnStart(){}

}
