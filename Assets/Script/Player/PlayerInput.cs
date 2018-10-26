﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FVRlib;
using UniRx;
using UniRx.Triggers;

public enum InputState
{
    _none = 0,
    _armUp,
    _armDown,
    _armLeft,
    _armRight,
    _armFront,
    _handUp,
    _handDown,
    _gestureRock,
    _gesturePaper,
    _accel,
}

public class PlayerInput : MonoBehaviour
{
    //------------------------------------
    // private
    //------------------------------------
    FVRConnection fvr;

    //------------------------------------
    // プロパティ
    //------------------------------------
    public InputState _InputState { get; private set; }


    //------------------------------------
    // 関数
    //------------------------------------
    void Start()
    {
        fvr = FindObjectOfType(typeof(FVRConnection)) as FVRConnection;

        InputObserver();
    }

    /// <summary>
    /// Inputの入力状態を監視
    /// </summary>
    void InputObserver()
    {
#if UNITY_EDITOR

        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetKeyDown(KeyCode.A))
            .Subscribe(_ => _InputState = InputState._armLeft);

        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetKeyDown(KeyCode.S))
            .Subscribe(_ => _InputState = InputState._armFront);

        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetKeyDown(KeyCode.D))
            .Subscribe(_ => _InputState = InputState._armRight);

        this.UpdateAsObservable()
             .TakeUntilDestroy(this)
            .Where(_ => Input.GetKeyDown(KeyCode.F))
            .Subscribe(_ => _InputState = InputState._armDown);

        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetKeyDown(KeyCode.G))
            .Subscribe(_ => _InputState = InputState._armUp);

        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetKeyDown(KeyCode.H))
            .Subscribe(_ => _InputState = InputState._accel);

#else 

        //一定速度以上で腕を動かしたとき
        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => fvr.accel.magnitude > 1.0f)
            .Subscribe(_ => _InputState = InputState._accel);

        //腕を下に向けているとき
        this.ObserveEveryValueChanged(playerInput => playerInput.fvr.palmOrientation)
            .TakeUntilDestroy(this)
            .Where(_ => fvr.horizontalOrientation == FVRConnection.HorizontalOrientation.front &&
                        fvr.palmOrientation == FVRConnection.PalmOrientation.down)
            .Subscribe(_ => _InputState = InputState._armFront);

        //腕を左に向けているとき
        this.ObserveEveryValueChanged(playerInput => playerInput.fvr.horizontalOrientation)
            .TakeUntilDestroy(this)
            .Where(_ => fvr.horizontalOrientation == FVRConnection.HorizontalOrientation.left)
            .Subscribe(_ => _InputState = InputState._armLeft);

        //腕を右に向けているとき
        this.ObserveEveryValueChanged(playerInput => playerInput.fvr.horizontalOrientation)
            .TakeUntilDestroy(this)
            .Where(_ => fvr.horizontalOrientation == FVRConnection.HorizontalOrientation.right)
            .Subscribe(_ => _InputState = InputState._armRight);

        //腕を上に向けているとき
        this.ObserveEveryValueChanged(playerInput => playerInput.fvr.verticalOrientation)
            .TakeUntilDestroy(this)
            .Where(_ => fvr.verticalOrientation == FVRConnection.VerticalOrientation.up)
            .Subscribe(_ => _InputState = InputState._armUp);

        //腕を下に向けているとき
        this.ObserveEveryValueChanged(playerInput => fvr.verticalOrientation)
            .TakeUntilDestroy(this)
            .Where(_ => fvr.verticalOrientation == FVRConnection.VerticalOrientation.down)
            .Subscribe(_ => _InputState = InputState._armDown);

#endif
    }
}

