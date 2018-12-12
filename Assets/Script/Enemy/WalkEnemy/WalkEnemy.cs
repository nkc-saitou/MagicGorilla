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

    private int currentPositionIndex=0;                     //目標値用
    private const float speed=2.0f;                         //移動速度
    private const float jumpPower = 5f;                     //飛ぶ力
    private const float ShiftRange=0.5f;
    private bool jumpSet;
    private Vector3 quaternion;
    float speedrot = 120f;


    [SerializeField,Tooltip("攻撃間隔")]
    private float attackWait;


    /// <summary>
    /// Startと同義
    /// </summary>
    protected override void OnStart()
    {
        walkAnim = GetComponent<WalkEnemyAnimator>();

        PathSet();


        this.FixedUpdateAsObservable().
            TakeUntilDestroy(this).
            Where(_ => Vector3.Distance(PlayerPos.position, transform.position) > 2.0f).
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
            Where(_ => _.IsName("Base Layer.Run")&& Vector3.Distance(PlayerPos.position, transform.position) >= 1.8f).
            Subscribe(_ => {
                PathSet();
                });

        stateMachineObservables.
            OnStateUpdateObservable.
            Where(_ => _.IsName("Base Layer.Set")&&enemyRay.OnGround).
            Subscribe(_ =>Rotate());

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
        Vector3 shift = Shift;
        agent.CalculatePath(PlayerPos.position+shift, path);
        agent.enabled = false;
    }

    Vector3 Shift//目標地点をずらす
    {
        get
        {
            Vector3 vector3=Vector3.zero;
            vector3.x = UnityEngine.Random.Range(ShiftRange, -ShiftRange);
            vector3.z = UnityEngine.Random.Range(ShiftRange, -ShiftRange);
            return vector3;
        }
    }

    /// <summary>
    /// パスの地点までの移動を繰り返す
    /// </summary>
    protected void Move()
    {
        if (EnemyHP > 0)
        {
            var targetPosition = path.corners[currentPositionIndex];//現在の目的地
                                                                    //終点に近いなら止まる
            if ((currentPositionIndex + 1 == path.corners.Length && Vector3.Distance(targetPosition, transform.position) < 1.5f)||
                 jumpSet   )
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
    }
    #endregion
}
