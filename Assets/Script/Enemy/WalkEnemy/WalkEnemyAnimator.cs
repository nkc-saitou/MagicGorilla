using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class WalkEnemyAnimator : MonoBehaviour {

    Animator anim;
    StateMachineObservalbes stateMachineObservables;
    Transform playerPos;

    void Start () {
        anim = GetComponent<Animator>();
        //stateMachineObservables = GetComponent<Animator>().GetBehaviour<StateMachineObservalbes>();

        if (!playerPos)//プレイヤーを探す
        {
            playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        }


        Observable.EveryUpdate().
            TakeUntilDestroy(this).
            Subscribe(_ => AnimationChanger());
    }

    void AnimationChanger()
    {
        if (anim)
        {
            if (Vector3.Distance(playerPos.position, transform.position) <= 2.0f)
            {
                anim.SetBool("near", true);
            }
            else
            {
                anim.SetBool("near", false);
            }
        }
    }

    public void Jump()
    {
        if (anim)
        {
            anim.SetTrigger("Jump");
        }
    }
}
