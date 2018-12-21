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
    public override void Charge(Transform pos, bool isFollow = true)
    {
        base.Charge(pos);
        PlayEffect();
    }

    public override void Shot(GameObject target)
    {
        base.Shot(target);
        Debug.Log("Sword");
    }

    
    public override void PlayEffect()
    {
        base.PlayEffect();

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
