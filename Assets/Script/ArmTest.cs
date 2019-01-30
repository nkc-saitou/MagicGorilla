using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using FVRlib;

public class ArmTest : MonoBehaviour {

    public Text inputState;
    public Text verticalOrientation;
    public Text horizontalOrientation;
    public Text Palm;

    FVRConnection fvr;

    FVRInputState playerInputState;

	// Use this for initialization
	void Start () {

        fvr = FindObjectOfType<FVRConnection>();

        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Subscribe(_ => playerInputState = GetComponent<PlayerInput>()._FVRInputState);

        gameObject.ObserveEveryValueChanged(_ => playerInputState)
            .Subscribe(x => inputState.text = x.ToString());

        gameObject.ObserveEveryValueChanged(_ => fvr.verticalOrientation)
            .Subscribe(x => verticalOrientation.text = x.ToString());

        gameObject.ObserveEveryValueChanged(_ => fvr.horizontalOrientation)
            .Subscribe(x => horizontalOrientation.text = x.ToString());

        gameObject.ObserveEveryValueChanged(_ => fvr.palmOrientation)
            .Subscribe(x => Palm.text = x.ToString());
    }
}
