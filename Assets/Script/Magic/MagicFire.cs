using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;


public class MagicFire : BaseMagic {

    public override void DoStart()
    {
        baseMagic = GetComponent<MagicFire>();
    }

    public override void DoUpdate()
    {

    }

    public override void Shot(Transform target)
    {
        base.Shot(target);
    }

    public override void Charge(Transform pos, Transform movePos = null, bool isEffectDisplay = true)
    {
        base.Charge(pos);
        PlayEffect(pos);
    }

    public override void PlayEffect(Transform pos, Transform movePos = null, bool isEffectDisplay = true)
    {
        base.PlayEffect(pos);

        this.UpdateAsObservable()
            .TakeUntilDestroy(effect)
            .Where(_ => effect != null)
            .Subscribe(_ =>
            {
                Vector3 tempVec = new Vector3(Mathf.Sin(Time.time * 20.0f), Mathf.Sin(Time.time * 20.0f), Mathf.Sin(Time.time * 20.0f));
                effect.transform.localScale = tempVec;
            });
    }
}