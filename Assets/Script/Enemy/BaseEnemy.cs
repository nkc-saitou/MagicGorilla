using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;
using UniRx.Triggers;


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
    [SerializeField,Tooltip("死ぬまでのロスタイム")]
    protected float deadLossTime = 3;

    //プレイヤーの座標
    protected Transform PlayerPos;
    protected bool dead;

    [SerializeField, Tooltip("ラグドール")]
    protected GameObject ragdoll;

    public virtual float EnemyHP
    {
        get { return enemyHp; }
        set {
            enemyHp = value >= 0 ? value : 0;   //HPがマイナスにならないように
            if (enemyHp == 0&&!dead)
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

        this.UpdateAsObservable().
            TakeUntilDestroy(this).
            Where(_ => transform.position.y<-20).
            Subscribe(_ => transform.position=new Vector3(transform.position.x,20,transform.position.z));

        if (!PlayerPos)//プレイヤーを探す
        {
            PlayerPos = GameObject.FindGameObjectWithTag("Player").transform;
        }
        StartRotate();
        OnStart();
    }

    /// <summary>
    /// 死亡処理
    /// </summary>
    protected virtual void Dead()
    {
        dead = true;
        GameObject.Find("ScoreManager").GetComponent<ScoreManager>().AddScore(score);
        anim.enabled = false;
        Instantiate(ragdoll, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    /// <summary>
    /// Startと同義
    /// </summary>
    protected virtual void OnStart(){}


    void StartRotate()
    {
        Vector3 direction = (PlayerPos.position - transform.position).normalized;
        Vector3 xAxis = Vector3.Cross(Vector3.up, direction).normalized;
        Vector3 zAxis = Vector3.Cross(xAxis, Vector3.up).normalized;
        transform.rotation = Quaternion.LookRotation(zAxis, Vector3.up);
    }

}
