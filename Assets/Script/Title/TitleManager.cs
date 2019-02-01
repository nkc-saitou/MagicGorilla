using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class TitleManager : MonoBehaviour {

    bool isFade = false;

    bool isOVRTriggerDown;
    bool isOVRTochPadDown;

    // Use this for initialization
    void Start()
    {
        this.UpdateAsObservable()
            .Subscribe(_ => 
            {
                isOVRTriggerDown = PlayerInput.Instance.IsOVRTriggerDown;
                isOVRTochPadDown = PlayerInput.Instance.IsOVRTochPadGetDown;
            });
    }

    // Update is called once per frame
    void Update()
    {
        if (isOVRTriggerDown && isFade == false)
        {
            PlayerInput.Instance.IsOVRTochPadGetDown = false;
            isFade = true;
            EffectManager.Instance.FadeScene("PlayerTest");
        }
    }
}
