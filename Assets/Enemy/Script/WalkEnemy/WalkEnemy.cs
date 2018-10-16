using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkEnemy : BaseEnemy
{
    private int currentPositionIndex=0;                     //目標値用
    private int myHp = 1;                                   //HP
    private const float speed=2.0f;                         //移動速度
    private const float rotSpeed = 5.0f;                    //回転速度
    private bool step=false;                                //目の前が段差か
    private bool moveFinish;                                //移動終了
    private bool onGround;                                  //接地判定

    Rigidbody rb;
    WalkEnemyRay enemyRay;
    LineRenderer Lr;

    protected override void OnStart()
    {
        //体力を設定
        EnemyHp = myHp;

        Lr = GetComponent<LineRenderer>();
        enemyRay = GetComponent<WalkEnemyRay>();
        rb = GetComponent<Rigidbody>();
        StartMove();
        LineRender();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        enemyRay.OnGround(out onGround);
        if (!moveFinish)
        {
            Move();
            if (onGround)
            {
                Rotate();
                enemyRay.StepDetection(out step);
                if (step)
                {
                    Jump();
                }
            }
        }
    }

    protected void StartMove()
    {
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
        onGround = false;
        rb.AddForce(Vector3.up * 270);
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
