using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public class MagicSword : BaseMagic {

    public override void DoStart()
    {
        baseMagic = GetComponent<MagicSword>();
    }

    public override void DoUpdate()
    {

    }

    public override void Shot(Transform target)
    {
        base.Shot(target);
        Debug.Log("Sword");
    }

    public override void Charge(Transform pos, Transform movePos = null, bool isEffectDisplay = true)
    {
        base.Charge(pos);
        PlayEffect(pos);
    }
    
    public override void PlayEffect(Transform pos, Transform movePos = null, bool isEffectDisplay = true)
    {
        base.PlayEffect(pos);

        if (effect.transform.childCount < 1) return;

        int count = 0;

        Observable.Interval(TimeSpan.FromMilliseconds(600))
            .TakeUntilDestroy(effect)
            .Take(5)
            .Subscribe(l =>
            {
                effect.transform.GetChild(count).gameObject.SetActive(true);
                count++;
            });
    }
}
