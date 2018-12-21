using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class WalkEnemyAnimator : BaseEnemy {


    protected override void OnStart () {
        var every = Observable.EveryUpdate();

        every.TakeUntilDestroy(this)
             .Subscribe(_ => AnimationChanger());
    }

    void AnimationChanger()
    {
        if(Vector3.Distance(PlayerPos.position, transform.position) <= 2.0f)
        {
            anim.SetBool("near", true);
        }
        else
        {
            anim.SetBool("near", false);
        }
    }
}
