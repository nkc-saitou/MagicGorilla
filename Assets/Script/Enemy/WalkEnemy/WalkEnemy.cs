using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkEnemy : BaseEnemy
{
    private int currentPositionIndex=0;                     //目標値用
    private const float speed=2.0f;                         //移動速度
    private const float rotSpeed = 5.0f;                    //回転速度
    private const float jumpPower = 5f;                     //飛ぶ力
    private bool step=false;                                //目の前が段差か
    private bool moveFinish;                                //移動終了

    Rigidbody rb;
    WalkEnemyRay enemyRay;
    LineRenderer Lr;



    /// <summary>
    /// Startと同義
    /// </summary>
    protected override void OnStart()
    {
        Lr = GetComponent<LineRenderer>();
        enemyRay = GetComponent<WalkEnemyRay>();
        rb = GetComponent<Rigidbody>();
        StartRotate();
        PathSet();
        LineRender();
    }



    void FixedUpdate()
    {
        if (moveFinish)
        {
            if (Vector3.Distance(PlayerPos.position, transform.position) > 2)
            {
                PathSet();
                moveFinish = false;
            }
        }
        else
        {
            enemyRay.StepDetection(out step);
            Move();
            if (enemyRay.OnGround)
            {
                Rotate();
                if (step)
                {
                    Jump();
                }
            }
        }
    }



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



    /// <summary>
    /// パスの地点までの移動を繰り返す
    /// </summary>
    protected void Move()
    {
        var targetPosition = path.corners[currentPositionIndex];//現在の目的地
        //終点に近いなら止まる
        if (currentPositionIndex + 1 == path.corners.Length&& Vector3.Distance(targetPosition, transform.position) < 2f)
        {
            moveFinish = true;
            return;
        }
        //目標値についたら次へ
        if (Vector3.Distance(new Vector3(targetPosition.x,transform.position.y,targetPosition.z), transform.position) < 0.5f)
        {
            //現在の数+1が長さより少ない
            currentPositionIndex = currentPositionIndex + 1 < path.corners.Length ? currentPositionIndex + 1 : currentPositionIndex;
        }
        transform.localPosition += transform.forward * speed * Time.deltaTime;
    }


    protected void Attack()
    {

    }



    /// <summary>
    /// ジャンプ
    /// </summary>
    protected void Jump()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.velocity=(Vector3.up * jumpPower);
    }



    void StartRotate()
    {
        Vector3 direction = (PlayerPos.position - transform.position).normalized;
        Vector3 xAxis = Vector3.Cross(Vector3.up, direction).normalized;
        Vector3 zAxis = Vector3.Cross(xAxis, Vector3.up).normalized;
        transform.rotation = Quaternion.LookRotation(zAxis,Vector3.up);
    }



    protected void Rotate()
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



    void LineRender()
    {
        //移動ルート描画
        Lr.positionCount=path.corners.Length;
        for(int i = 0; i < path.corners.Length; i++)
        {
            Vector3 corner = path.corners[i];
            Lr.SetPosition(i, corner + Vector3.up);
        }
    }
}
