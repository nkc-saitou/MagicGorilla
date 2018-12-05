﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Linq;

abstract public class BaseMagic : MonoBehaviour, IAction
{
    //---------------------------------------------
    // public
    //---------------------------------------------

    public GameObject magicEffect;

    public float Pow { get; protected set; }


    public float MaxPow = 5;

    // 弾を打ったタイミングを公開
    public IObservable<Transform> OnShot
    {
        get { return shotSubject; }
    }

    //---------------------------------------------
    // protected
    //---------------------------------------------

    //弾を打った時のタイミングを通知
    protected Subject<Transform> shotSubject = new Subject<Transform>();

    protected GameObject effect;

    protected BaseMagic baseMagic;
    //---------------------------------------------
    // private
    //---------------------------------------------

    Transform targetPos;
    Transform effectPos;

    //---------------------------------------------
    // 抽象メソッド
    //---------------------------------------------

    public abstract void DoStart();

    public abstract void DoUpdate();

    //---------------------------------------------
    // 関数
    //---------------------------------------------
    void Start()
    {
        DoStart();
    }

    void Update()
    {
        DoUpdate();
    }

    /// <summary>
    /// 魔法を生成
    /// </summary>
    /// <param name="pos">魔法を生成する位置</param>
    public virtual void Charge(Transform pos, Transform movePos = null, bool isEffectDisplay = true)
    {
        Pow = Mathf.Min(Pow += Time.deltaTime, MaxPow);

        effectPos = pos;
    }

    /// <summary>
    /// 魔法を発射
    /// </summary>
    /// <param name="pos"></param>
    public virtual void Shot(Transform pos)
    {
        if (pos == null) return;
        shotSubject.OnNext(pos);
    }

    public virtual void PlayEffect(Transform pos, Transform movePos = null, bool isEffectDisplay = true)
    {
        if (effect != null) return;

        Debug.Log(pos);
        effect = Instantiate(magicEffect, pos.position, magicEffect.transform.rotation);
        BaseMagicBullet magic = effect.GetComponent<BaseMagicBullet>();
        magic._BaseMagic = baseMagic;
    }
}
