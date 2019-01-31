using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System;

public enum CreateHandType
{
    noneRock,
    nonePaper,
    upRock,
    upPaper,
    downRock,
    downPaper
}

public enum CreateAttributeType
{
    none,
    up,
    down
}

public class AttackHandShot : MonoBehaviour {

    //-----------------------------------------------------
    // public
    //-----------------------------------------------------

    [Tooltip("0:noneRock、1:nonePaper、2:UpRock、3:UpPaper、4:DownRock、5:DownPaper")]
    public GameObject[] attributePre;

    public Transform CreatePos;

    public CreateAttributeType CreateType { get; private set; }

    //-----------------------------------------------------
    // private
    //-----------------------------------------------------

    FVRInputState fvrState;

    //OVRInputState ovrState;

    bool isOVRTriggerDown;
    bool isOVRTochPadDown;

    bool isInterbar = false; // インターバル中かどうか

    GestureInputState gestureState;


    // Use this for initialization
    void Start ()
    {

        //毎フレームInputStateを監視
        this.UpdateAsObservable()
            .Subscribe(_ => fvrState = PlayerInput.Instance._FVRInputState);

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                isOVRTriggerDown = PlayerInput.Instance.IsOVRTriggerDown;
                isOVRTochPadDown = PlayerInput.Instance.IsOVRTochPadGetDown;

                SetAttributeType();
            });

        this.UpdateAsObservable()
            .Subscribe(_ => gestureState = PlayerInput.Instance._GestureInputState);


        PlayerInput.Instance.OnAccel
            .Where(_ => isInterbar == false)
            .Subscribe(_ =>
            {
                isInterbar = true;

                HandShot();

                Interbar();
            });


        //this.UpdateAsObservable()
        //    .Where(_ => PlayerInput.Instance.IsFVRAccel)
        //    .Subscribe(_ =>
        //    {
        //        HandShot();
        //    });


    }

    void SetAttributeType()
    {
        switch(fvrState)
        {
            case FVRInputState.armUp:
                CreateType = CreateAttributeType.up;
                break;

            case FVRInputState.armDown:
                CreateType = CreateAttributeType.down;
                break;
        }

        if (isOVRTriggerDown)
        {
            CreateType = CreateAttributeType.none;
            PlayerInput.Instance.InitFVRInputState();
        }
    }

    /// <summary>
    /// Handオブジェクト発射！
    /// </summary>
    void HandShot()
    {
        GameObject tempHandObj = null;

        switch (CreateType)
        {
            case CreateAttributeType.up:
                if (gestureState == GestureInputState.gestureRock)
                    tempHandObj = attributePre[(int)CreateHandType.upRock]; 
                else
                    tempHandObj = attributePre[(int)CreateHandType.upPaper];
                break;

            case CreateAttributeType.down:
                if (gestureState == GestureInputState.gestureRock)
                    tempHandObj = attributePre[(int)CreateHandType.downRock];
                else
                    tempHandObj = attributePre[(int)CreateHandType.downPaper];
                break;

            default:
                if (gestureState == GestureInputState.gestureRock)
                    tempHandObj = attributePre[(int)CreateHandType.noneRock];
                else
                    tempHandObj = attributePre[(int)CreateHandType.nonePaper];
                break;
        }

        if (tempHandObj != null)
        {
            Instantiate(tempHandObj, CreatePos.position, tempHandObj.transform.rotation);
            fvrState = FVRInputState.armFront;
        }
    }

    void Interbar()
    {
        StartCoroutine(DelayMethod(0.5f, () =>
        {
            isInterbar = false;
        }));
    }

    private IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}