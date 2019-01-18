using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class MagicRock : BaseMagic {

    [Header("0 : プレイヤー　, 1 : エネミー")]
    public Transform[] magicCreatePos;

    public override void DoStart()
    {
        baseMagic = GetComponent<MagicRock>();
    }

    public override void DoUpdate()
    {

    }

    public override void Charge(Transform pos, bool isFollow = true)
    {
        base.Charge(pos);

        //PlayEffect();
    }

    public override void Shot(GameObject target)
    {
        base.Shot(target);

        GameObject attackMagic = Instantiate(attackEffect, magicCreatePos[0].position, attackEffect.transform.rotation);
        GameObject attackCol = Instantiate(magicCol, magicCreatePos[0].position, magicCol.transform.rotation);

        Destroy(attackMagic, 5.0f);
        Destroy(attackCol, 2.5f);
    }

    public override void PlayEffect()
    {

    }
}