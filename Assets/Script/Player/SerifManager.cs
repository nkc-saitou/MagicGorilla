using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class SerifManager : MonoBehaviour {

    public GestureSetting gestureSetting;

    public string[] rootSerif;

    public Text serifText;

    public Animator anim;

    int rootSerifIndex = 0;

    int setCount = 0;

    bool isSerifInterbar = false; //インターバル中かどうか

    bool isSerifSend = true; //セリフ送りをしても良いか

    float time;

    void Start()
    {
        this.ObserveEveryValueChanged(_ => rootSerifIndex)
            .Where(_ => rootSerif.Length > rootSerifIndex)
            .Subscribe(_ => serifText.text = rootSerif[rootSerifIndex]);

        anim.SetTrigger("IsStartChat");
    }

    private void Update()
    {
        if ((OVRInput.Get(OVRInput.Button.Right) || Input.GetKeyDown(KeyCode.I)) && isSerifInterbar == false)
        {
            rootSerifIndex++;

            isSerifInterbar = true;
        }

        if(isSerifInterbar == true) TimeInterbar();

        if (rootSerif.Length == rootSerifIndex) anim.SetTrigger("IsEndChat");


    }

    void TimeInterbar()
    {
        time += Time.deltaTime;

        if(time >= 0.5f)
        {
            isSerifInterbar = false;
            time = 0.0f;
        }
    }
}
