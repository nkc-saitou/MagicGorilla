using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyRay))]
public abstract class BaseEnemy : MonoBehaviour {


    protected NavMeshAgent agent;
    protected EnemyRay enemyRay;
    protected Rigidbody rb;
    protected Animator anim;
    protected StateMachineObservalbes stateMachineObservables;



    //移動用パス
    protected NavMeshPath path;


    protected float enemyHp=1;
    protected int score=10;

    //プレイヤーの座標
    protected Transform PlayerPos;


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
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        stateMachineObservables = GetComponent<Animator>().GetBehaviour<StateMachineObservalbes>();
        enemyRay = GetComponent<EnemyRay>();
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
