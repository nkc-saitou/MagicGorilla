using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.AI;
using System;

public class WalkEnemy : BaseEnemy
{
    WalkEnemyAnimator walkAnim;
    AttackAllocation allocation;

    int currentPositionIndex=0;                     //目標値用
    const float speed=2.0f;                         //移動速度
    const float jumpPower = 5f;                     //飛ぶ力
    const float ShiftRange=0.5f;
    bool jumpSet;
    Vector3 quaternion;
    Vector3 targetPosition;
    float speedrot = 120f;


    [SerializeField,Tooltip("攻撃間隔")]
    private float attackWait;



    /// <summary>
    /// Startと同義
    /// </summary>
    protected override void OnStart()
    {
        walkAnim = GetComponent<WalkEnemyAnimator>();
        allocation = GameObject.Find("AttackAllocation").GetComponent<AttackAllocation>();

        PathSet();


        this.FixedUpdateAsObservable().
            TakeUntilDestroy(this).
            Where(_ => Vector3.Distance(PlayerPos.position, transform.position) > 1.5f).
            Subscribe(_ => Move());


        var interval = Observable.Interval(System.TimeSpan.FromSeconds(attackWait));
        interval.
            TakeUntilDestroy(this).
            Where(_ => Vector3.Distance(PlayerPos.position, transform.position) <= 2.0f).
            Subscribe(_ => Attack());


        stateMachineObservables.
            OnStateEnterObservable.
            Where(_ => _.IsName("Base Layer.Attack")).
            Subscribe(_ =>
                {
                    quaternion = transform.eulerAngles;
                    quaternion.y -= 40;//剣が当てる位置まで回転させる
                });

        stateMachineObservables.
            OnStateEnterObservable.
            TakeUntilDestroy(this).
            Where(_ => _.IsName("Base Layer.Run")&& Vector3.Distance(PlayerPos.position, transform.position) >= 1.8f).
            Subscribe(_ => {
                PathSet();
                });

        //プレイヤーをに向く
        stateMachineObservables.
            OnStateUpdateObservable.
            Where(_ => _.IsName("Base Layer.Set")&&enemyRay.OnGround).
            Subscribe(_ => {
                targetPosition = PlayerPos.position;
                Rotate();
                });

        stateMachineObservables.
            OnStateUpdateObservable.
            Where(_ => _.IsName("Base Layer.Attack")).
            Subscribe(_ => AttackRotate());

        stateMachineObservables.
            OnStateEnterObservable.
            Where(_ => _.IsName("Base Layer.Jump")).
            Subscribe(_ => JumpSet());

    }

    protected void Attack()
    {

    }

    protected void AttackRotate()
    {
        if (EnemyHP > 0)
        {
            float step = speedrot * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(quaternion), step);
        }
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
        targetPosition = path.corners[currentPositionIndex];
    }



    protected void Move()
    {
        if (EnemyHP > 0)
        {
            if (jumpSet)
            {
                return;
            }
            else
            {
                PositionSet();
                transform.localPosition += transform.forward * speed * Time.deltaTime;
            }

            if (enemyRay.OnGround)
            {
                Rotate();
                if (enemyRay.StepDetection)
                {
                    walkAnim.Jump();
                }
            }
        }
    }

    void PositionSet()
    {
        //目標値についたら次へ
        if (Vector3.Distance(new Vector3(targetPosition.x, transform.position.y, targetPosition.z), transform.position) < 4.5f)
        {
            //現在の数+1が長さより少ない
            if (currentPositionIndex < path.corners.Length)
            {
                currentPositionIndex++;
                if (currentPositionIndex < path.corners.Length)
                    targetPosition = path.corners[currentPositionIndex];
            }
            else if (currentPositionIndex == path.corners.Length)
            {
                targetPosition = allocation.PositionCheck(gameObject);
                if (targetPosition == Vector3.zero)
                {
                    targetPosition = PlayerPos.position;
                }
            }
        }
    }


    #region Jump
    void JumpSet()
    {
        jumpSet = true;
        Observable.Timer(System.TimeSpan.FromSeconds(0.4f)).
            Subscribe(_ =>
            {
                jumpSet = false;
                Jump();
            });
    }
    protected void Jump()
    {
        if (EnemyHP > 0)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.velocity = (Vector3.up * jumpPower);
        }
    }
    #endregion

    protected void Rotate()
    {
        if (EnemyHP > 0)
        {   //Y軸回転
            Vector3 direction = (targetPosition - transform.position).normalized;
            Vector3 xAxis = Vector3.Cross(Vector3.up, direction).normalized;
            Vector3 zAxis = Vector3.Cross(xAxis, Vector3.up).normalized;
            Vector3 newdir = Vector3.RotateTowards(transform.forward, zAxis, 5 * Time.deltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(newdir);
        }
    }
    #endregion
}
