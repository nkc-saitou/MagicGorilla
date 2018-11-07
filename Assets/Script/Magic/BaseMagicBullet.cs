using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(ObjectCollision))]
public class BaseMagicBullet : MonoBehaviour
{

    //---------------------------------------------
    // public
    //---------------------------------------------
    public BaseMagic _BaseMagic { get; set; }

    //---------------------------------------------
    // protected
    //---------------------------------------------

    protected ObjectCollision collision;

    protected float speed = 0.1f;

    protected bool isShot = false;

    protected Transform pos = null;

    protected virtual void Start()
    {

        MagicCollision();

        BulletMove();

        //弾を打ったら
        _BaseMagic.OnShot
            .TakeUntilDestroy(_BaseMagic)
            .Where(_ => isShot == false)
            .Where(targetPos => targetPos.gameObject != null)
            .Subscribe(targetPos =>
            {
                pos = targetPos;
                isShot = true;
            });

        //当たった時の処理
        collision.OnCollision
            .TakeUntilDestroy(this)
            .Where(colObj => colObj.GetComponent<BaseEnemy>() != null)
            .Subscribe(_ => Destroy(gameObject));

        //弾が誰にも当たらなかった場合は一定時間後に削除
        Observable.Timer(TimeSpan.FromSeconds(3))
            .TakeUntilDestroy(gameObject)
            .Where(_ => isShot == true)
            .Subscribe(_ => Destroy(gameObject));

    }

    /// <summary>
    /// 弾が動く
    /// </summary>
    protected virtual void BulletMove()
    {
        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => isShot == true)
            .Subscribe(_ =>
            {
                transform.LookAt(pos);
                transform.position += transform.forward * speed;
            });
    }

    /// <summary>
    /// 自分の当たり判定を取得
    /// </summary>
    protected virtual void MagicCollision()
    {
        collision = GetComponent<ObjectCollision>();
    }
}
