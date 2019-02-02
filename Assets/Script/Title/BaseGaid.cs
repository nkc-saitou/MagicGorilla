using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public abstract class BaseGaid : MonoBehaviour {

    protected Animator anim;

    bool isCanAnim; //アニメーションさせても良いか

    bool isTemp;

    // Use this for initialization
    void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
        GaidAnim();

        DoStart();
    }

    // Update is called once per frame
    void Update()
    {
        DoUpdate();

        AnimationStart();
    }


    public void GaidAnim()
    {
        if (PlayerInput.Instance.HitGameObject == null) return;

        GameObject choiceGaid = PlayerInput.Instance.HitGameObject.transform.parent.gameObject;

        if(choiceGaid == gameObject)
        {
            isCanAnim = true;
        }
        else
        {
            isCanAnim = false;
        }
    }

    void AnimationStart()
    {
        if (anim.enabled == false) return;

        if (isCanAnim)
        { 
            anim.SetBool("IsChoce", true);
        }
        else if(anim.GetBool("IsChoce") == true)
        {
            anim.SetBool("IsChoce", false);
        }
    }

    protected abstract void DoStart();

    protected abstract void DoUpdate();

    public abstract void GaidAction();
}
