using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class GolemAnimator : MonoBehaviour
{
    protected Animator anim;
    protected StateMachineObservalbes stateMachineObservables;
    protected string[] attackName = { "Punch", "Shoot" };

    void Start()
    {
        anim = GetComponent<Animator>();
        stateMachineObservables = GetComponent<Animator>().GetBehaviour<StateMachineObservalbes>();
        anim.SetBool("Shoot", true);

        stateMachineObservables.
            OnStateExitObservable.
            TakeUntilDestroy(this).
            Where(_ => _.IsName("Base Layer.Set")).
            Subscribe(_ => AnimationSet());

        stateMachineObservables.
            OnStateEnterObservable.
            TakeUntilDestroy(this).
            Where(_ => _.IsName("Damage.DamageL")).
            Subscribe(_ => anim.applyRootMotion = false);

        stateMachineObservables.
            OnStateEnterObservable.
            TakeUntilDestroy(this).
            Where(_ => _.IsName("Damage.DamageR")).
            Subscribe(_ => anim.applyRootMotion = false);
    }

    void AnimationSet()
    {
        anim.applyRootMotion = true;
        anim.SetBool("Left", !anim.GetBool("Left"));
        foreach (var name in attackName)
        {
            anim.SetBool(name, false);
        }
        int i = Random.Range(0, attackName.Length);
        anim.SetBool(attackName[i], true);
    }

    public void Damage()
    {
        anim.SetTrigger("Damage");
    }
}
