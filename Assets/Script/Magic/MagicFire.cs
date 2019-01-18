using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;


public class MagicFire : BaseMagic {

    public Transform[] createPos;
    public Transform parent;

    GameObject magicEff;

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

        if(effect != null)effect.transform.parent = null;

        Destroy(magicEff, 0.2f);
        Destroy(effect, 1.0f);
    }

    public override void PlayEffect()
    {
        base.PlayEffect();

        effect.transform.parent = parent;
        magicEff = Instantiate(attackEffect, createPos[0].position, attackEffect.transform.rotation, parent);



        OnShot.Subscribe(_ =>
        {
            SphereCollider magicCol = effect.GetComponent<SphereCollider>();
            magicCol.enabled = true;

        });
    }
}