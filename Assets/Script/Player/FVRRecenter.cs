using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FVRlib;
using UniRx;
using UniRx.Triggers;

public class FVRRecenter : MonoBehaviour {

    bool isOVRTochPadDown;

    FVRConnection fvr;

	// Use this for initialization
	void Start ()
    {

        fvr = FindObjectOfType<FVRConnection>();

        //毎フレームInputStateを監視
        this.UpdateAsObservable()
            .Subscribe(_ => isOVRTochPadDown = PlayerInput.Instance.IsOVRTochPadGetDown);

        this.UpdateAsObservable()
            .Where(_ => isOVRTochPadDown)
            .Subscribe(_ =>
            {
                if(fvr != null)
                {
                    fvr.Recenter();
                }
            });
    }
}
