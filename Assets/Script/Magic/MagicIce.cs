using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class MagicIce : BaseMagic {

    public GameObject magicCirclePre;
    GameObject circle = null;

    public override void DoStart()
    {
        baseMagic = GetComponent<MagicIce>();
    }

    public override void DoUpdate()
    {

    }

    public override void Shot(Transform target)
    {
        base.Shot(target);

        Debug.Log("Ice");
    }

    public override void Charge(Transform pos, Transform movePos = null, bool isEffectDisplay = true)
    {
        base.Charge(pos);

        if (circle != null)
        {
            this.UpdateAsObservable()
                .Subscribe(_ => PlayEffect(pos, movePos,isEffectDisplay));

            return;
        }

        Transform circleCashTransform = magicCirclePre.transform;

        circle = Instantiate(magicCirclePre, pos.position, circleCashTransform.rotation);

    }

    public override void PlayEffect(Transform pos, Transform movePos = null, bool isEffectDisplay = true)
    {
        circle.SetActive(isEffectDisplay);
        circle.transform.position = movePos.position;
    }
}
