﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class AllowEnemy : BaseEnemy {

    [SerializeField]
    private GameObject allow;
    [SerializeField]
    private GameObject shotPos;
    [SerializeField]
    private Transform eye;

    private const float wait = 1f;                           //発射間隔
    private const float speed = 2.0f;                        //移動速度
    private const float jumpPower = 5f;                      //飛ぶ力

    private int currentPositionIndex = 0;                    //目標値用
    private bool moveFinish;                                 //移動終了

    private AllowEnemyAttack allowAttack;

    protected override void OnStart()
    {
        allowAttack = GetComponent<AllowEnemyAttack>();
        StartRotate();
        PathSet();

        stateMachineObservables.
            OnStateEnterObservable.
            TakeUntilDestroy(this).
            Where(_ => _.IsName("Base Layer.Run")).
            Subscribe(_ =>{
                PathSet();
            });

        stateMachineObservables.
            OnStateEnterObservable.
            TakeUntilDestroy(this).
            Throttle(TimeSpan.FromSeconds(0.5f)).
            Where(_ => _.IsName("Base Layer.Attack")).
            Subscribe(_=> {
                Attack();
                });


        this.FixedUpdateAsObservable().
            TakeUntilDestroy(this).
            Where(_=> enemyRay.InSightTarget(eye, PlayerPos)&&enemyRay.OnGround).
            Subscribe(_ => {
                Rotate();
                anim.SetBool("InSight", true);
            });

        this.FixedUpdateAsObservable().
            TakeUntilDestroy(this).
            Where(_ => !enemyRay.InSightTarget(eye, PlayerPos)||!enemyRay.OnGround).
            Subscribe(_ => {
                anim.SetBool("InSight", false);
                Move();
            });
    }


    #region 移動
    /// <summary>
    /// パスの設定
    /// </summary>
    protected void PathSet()
    {
        agent.enabled = true;
        path = null;
        currentPositionIndex = 0;
        path = agent.path;
        agent.CalculatePath(PlayerPos.position, path);
        agent.enabled = false;

    }

    void StartRotate()
    {
        Vector3 direction = (PlayerPos.position - transform.position).normalized;
        Vector3 xAxis = Vector3.Cross(Vector3.up, direction).normalized;
        Vector3 zAxis = Vector3.Cross(xAxis, Vector3.up).normalized;
        transform.rotation = Quaternion.LookRotation(zAxis, Vector3.up);
    }

    /// <summary>
    /// パスの地点までの移動を繰り返す
    /// </summary>
    protected void Move()
    {
        var targetPosition = path.corners[currentPositionIndex];//現在の目的地
        if (currentPositionIndex + 1 == path.corners.Length && Vector3.Distance(targetPosition, transform.position) < 1.8f)
        {
            return;
        }
        //目標値についたら次へ
        if (Vector3.Distance(new Vector3(targetPosition.x, transform.position.y, targetPosition.z), transform.position) < 0.5f)
        {
            //現在の数+1が長さより少ない
            currentPositionIndex = currentPositionIndex + 1 < path.corners.Length ? currentPositionIndex + 1 : currentPositionIndex;
        }
        transform.localPosition += transform.forward * speed * Time.deltaTime;


        //ジャンプ
        if (enemyRay.OnGround)
        {
            MoveRotate();
            if (enemyRay.StepDetection)
            {
                Jump();
            }
        }
    }

    /// <summary>
    /// ジャンプ
    /// </summary>
    protected void Jump()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.velocity = (Vector3.up * jumpPower);
    }


    protected void MoveRotate()
    {
        var targetPosition = path.corners[currentPositionIndex];//現在の目的地
        //Y軸回転
        Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 xAxis = Vector3.Cross(Vector3.up, direction).normalized;
        Vector3 zAxis = Vector3.Cross(xAxis, Vector3.up).normalized;
        //ゆっくり回る
        Vector3 newdir = Vector3.RotateTowards(transform.forward, zAxis, 5 * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(newdir);
    }
    #endregion

    /// <summary>
    /// 回転
    /// </summary>
    protected void Rotate()
    {
        Vector3 direction = (PlayerPos.position - transform.position).normalized;
        Vector3 xAxis = Vector3.Cross(Vector3.up, direction).normalized;
        Vector3 zAxis = Vector3.Cross(xAxis, Vector3.up).normalized;

        Vector3 newdir = Vector3.RotateTowards(transform.forward, zAxis, 5 * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(newdir);
    }



    protected void Attack()
    {
        allowAttack.Attack(allow, PlayerPos.position);
    }

}
