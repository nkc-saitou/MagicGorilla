using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class SwordMagicBullet : BaseMagicBullet
{
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }

    protected override void BulletMove()
    {
        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => isShot == true)
            .Where(_ => transform.childCount >= 1)
            .Subscribe(_ =>
            {
                foreach (Transform child in transform)
                {
                    child.transform.LookAt(pos);
                    child.transform.position += child.transform.forward * speed;
                }
            });
    }

    protected override void MagicCollision()
    {
        collision = transform.GetChild(0).GetComponent<ObjectCollision>();
    }
}
