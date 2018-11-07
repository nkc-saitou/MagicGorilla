﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class MagicRock : BaseMagic {

    public override void DoStart()
    {
        baseMagic = GetComponent<MagicRock>();
    }

    public override void DoUpdate()
    {

    }

    public override void Shot(GameObject target)
    {
        base.Shot(target);

        Debug.Log("Rock");
    }

    public override void Charge(Transform pos)
    {
        base.Charge(pos);

        PlayEffect();
    }

    public override void PlayEffect()
    {
        base.PlayEffect();

        float tempScale = effect.transform.localScale.x;

        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => effect != null)
            .Subscribe(_ =>
            {
                Vector3 tempVec = new Vector3(Mathf.Sin(Time.time * 20.0f), Mathf.Sin(Time.time * 20.0f), Mathf.Sin(Time.time * 20.0f));
                effect.transform.localScale = tempVec;
            });
    }
}
