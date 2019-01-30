using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

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

    OVRInputState ovrState;

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
                ovrState = PlayerInput.Instance._OVRInputState;
                SetAttributeType();
            });

        this.UpdateAsObservable()
            .Subscribe(_ => gestureState = PlayerInput.Instance._GestureInputState);

        this.UpdateAsObservable()
            .Where(_ => fvrState == FVRInputState.accel)
            .Subscribe(_ =>
            {
                HandShot();
            });
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

        if (ovrState == OVRInputState.ovrGetDownTrigger) CreateType = CreateAttributeType.none;
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

        if(tempHandObj != null) Instantiate(tempHandObj, CreatePos.position, tempHandObj.transform.rotation);
    }
}
