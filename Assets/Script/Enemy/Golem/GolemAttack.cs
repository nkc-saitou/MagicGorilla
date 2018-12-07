using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class GolemAttack : BaseEnemy {
    [SerializeField]
    GameObject rock;
    [SerializeField]
    Transform armTipL;
    [SerializeField]
    Transform armTipR;

    Transform startpos;
    GolemAnimator golemAnim;

    private const int layerMask = ~(1 << 9);
    float deg =10;
    float distance;
    bool shoot;


    public override float EnemyHP
    {
        get { return enemyHp; }
        set
        {
            enemyHp = value >= 0 ? value : 0;   //HPがマイナスにならないように
            golemAnim.Damage();
            if (enemyHp == 0)
            {
                Dead();
            }
        }
    }

    protected override void OnStart()
    {
        enemyHp = 30;
        golemAnim = GetComponent<GolemAnimator>();
        var a = transform.position;
        var b = PlayerPos.position;
        a.y = 0;
        b.y = 0;
        distance = Vector3.Distance(a, b)-2;



        stateMachineObservables.
            OnStateUpdateObservable.
            TakeUntilDestroy(this).
            Where(_ => _.IsName("Base Layer.Set")).
            Subscribe(_ => Rotate());



        stateMachineObservables.
            OnStateEnterObservable.
            TakeUntilDestroy(this).
            Where(_ => _.IsName("Shoot.ShootL")).
            Subscribe(_ =>
            {
                shoot = true;
                StartCoroutine(Rock(armTipL));
            });

        stateMachineObservables.
            OnStateExitObservable.
            Where(_ => _.IsName("Shoot.ShootL")).
            Subscribe(_ => shoot = false);

        stateMachineObservables.
            OnStateEnterObservable.
            TakeUntilDestroy(this).
            Where(_ => _.IsName("Shoot.ShootR")).
            Subscribe(_ =>
            {
                shoot = true;
                StartCoroutine(Rock(armTipR));
            });

        stateMachineObservables.
            OnStateExitObservable.
            Where(_ => _.IsName("Shoot.ShootR")).
            Subscribe(_ => shoot = false);
    }

    void Punch()
    {
         
    }

    #region 岩戸橋
    IEnumerator Rock(Transform arm)
    {
        yield return new WaitForSeconds(1.7f);
        for (int i = 0; i < 4; ++i)
        {
            yield return new WaitForSeconds(0.31f);
            if (!shoot) yield break;
            yield return null;
            RockBlast(arm,i);
        }
        yield break;
    }

    private void RockBlast(Transform arm,int i)
    {
        if (!shoot) return;
        GameObject rockObj = Instantiate(rock, arm.position, Quaternion.identity);
        if (i == 2)
        {
            targetpos = PlayerPos.position;
        }
        else
        {
            targetpos = transform.position;
            targetpos += distance * arm.transform.forward;
        }
        // 射出速度を算出
        Vector3 velocity = CalculateVelocity(arm.position, targetpos, rockObj, deg);

        if (rockObj)
        {
            // 射出
            Rigidbody rid = rockObj.GetComponent<Rigidbody>();
            rid.AddForce(velocity * rid.mass, ForceMode.Impulse);
        }
    }


    /// <summary>
    /// 標的に命中する射出速度の計算
    /// </summary>
    /// <param name="pointA">射出開始座標</param>
    /// <param name="pointB">標的の座標</param>
    /// <param name="rockObj">飛ばすもの</param>
    /// <returns>射出速度</returns>
    private Vector3 CalculateVelocity(Vector3 pointA, Vector3 pointB, GameObject rockObj,float angle)
    {
        // ラジアンに変換
        float rad = angle * Mathf.PI / 180;

        // 水平方向の距離x
        float x = Vector2.Distance(new Vector2(pointA.x, pointA.z), new Vector2(pointB.x, pointB.z));

        // 垂直方向の距離y
        float y = pointA.y - pointB.y;

        float speed = Mathf.Sqrt(rockObj.GetComponent<RockShoot>().localGravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(speed))
        {
            Destroy(rockObj);
            return Vector3.zero;
        }
        else
        {
            return (new Vector3(pointB.x - pointA.x, x * Mathf.Tan(rad), pointB.z - pointA.z).normalized * speed);
        }
    }
    #endregion

    protected void Rotate()
    {
        var targetPosition = PlayerPos.position;//現在の目的地
        //Y軸回転
        Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 xAxis = Vector3.Cross(Vector3.up, direction).normalized;
        Vector3 zAxis = Vector3.Cross(xAxis, Vector3.up).normalized;
        Vector3 newdir = Vector3.RotateTowards(transform.forward, zAxis, 2 * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(newdir);
    }

}
