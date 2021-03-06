﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class MagicIce : BaseMagic {

    public GameObject magicPre;

    GameObject magicCircle;

    Vector3 rayPos;

    bool isCircleMove = true;

    public override void DoStart()
    {
        baseMagic = GetComponent<MagicIce>();
    }

    public override void DoUpdate()
    {

    }
    public override void Charge(Transform pos, bool isFollow)
    {
        base.Charge(pos);

        isCircleMove = true;

        PlayEffect();
    }

    public override void Shot(GameObject target)
    {
        base.Shot(target);

        isCircleMove = false;

        Vector3 createPos = new Vector3(magicCircle.transform.position.x, magicCol.transform.position.y, magicCircle.transform.position.z);

        GameObject attackCol = Instantiate(magicCol, createPos, magicCol.transform.rotation);

        GameObject effect = Instantiate(attackEffect, magicCircle.transform.position, attackEffect.transform.rotation);

        Destroy(effect, 3.0f);
        Destroy(magicCircle, 3.0f);
        Destroy(attackCol, 0.5f);

        Debug.Log("Ice");
    }


    public override void PlayEffect()
    {
        if (magicCircle == null) magicCircle = Instantiate(magicPre, new Vector3(0, 0, 0), magicPre.transform.rotation);

        Vector3 rayPos = new Vector3(0, 0, 0);

        GameObject hitObject = null;
        MagicCreateCollision circleCreateCol = null;

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                hitObject = PlayerInput.Instance.HitGameObject;
                if (hitObject != null) circleCreateCol = hitObject.GetComponent<MagicCreateCollision>();
                rayPos = PlayerInput.Instance.RayHitPos;
            });

        this.UpdateAsObservable()
            .TakeUntilDestroy(magicCircle)
            .Where(_ => hitObject != null && circleCreateCol != null)
            .Where(_ => circleCreateCol.type == MagicStageType.Ice)
            .Where(_ => isCircleMove == true)
            .Subscribe(_ => magicCircle.transform.position = rayPos);
    }
}
