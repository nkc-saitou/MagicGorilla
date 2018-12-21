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
    public override void Charge(Transform pos, bool isFollow = true)
    {
        base.Charge(pos);

        PlayEffect();
    }

    public override void Shot(GameObject target)
    {
        base.Shot(target);
    }

    public override void PlayEffect()
    {
        base.PlayEffect();
    }
}
