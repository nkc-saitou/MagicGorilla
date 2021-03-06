﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FVRlib;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class GestureSetting : MonoBehaviour {

    FVRConnection fvr;
    FVRGesture gesture;

    int roundLength;

    public Image targetImg;
    public Image nonTargetImg;

    bool isFirst = false;

    public Subject<string> endCalibSub = new Subject<string>();

    public IObservable<string> OnEndCalib { get { return endCalibSub; } }

    public bool IsStart { get; private set; }

    void Start()
    {
        IsStart = false;

        fvr = FindObjectOfType(typeof(FVRConnection)) as FVRConnection;
        gesture = fvr.gestureManager.RegisterCustomGesture("gestureName");

        roundLength = (int)fvr.gestureManager.calibrationRoundLength;
    }

    public void SetTargetPress()
    {
        StartCoroutine(Calibrate(true));
    }

    public void SetNonTargetPress()
    {
        StartCoroutine(Calibrate(false));
    }

    public void ResetCalibrationPress()
    {
        fvr.gestureManager.ResetPatternData(gesture);
    }

    IEnumerator Calibrate(bool target)
    {
        if (target)
        {
            fvr.gestureManager.SetTargetData(gesture);
        }
        else
        {
            fvr.gestureManager.SetNonTargetData(gesture);
        }

        float t = 0;

        while (gesture.registering)
        {
            t += Time.deltaTime;
            if (target)
            {
                targetImg.fillAmount = t / roundLength;
            }
            else
            {
                nonTargetImg.fillAmount = t / roundLength;
            }
            yield return null;
        }

        yield return new WaitWhile(() => Fillamount(target));

        if (target) endCalibSub.OnNext("rock");
        else endCalibSub.OnNext("paper");

        targetImg.fillAmount = 0;
        nonTargetImg.fillAmount = 0;


        PlayerInput.Instance._FVRGesture = gesture;

    }

    bool Fillamount(bool isTarget)
    {
        targetImg.fillAmount += Time.deltaTime;

        if (targetImg.fillAmount >= 1.0f) return false;
        return true;
    }
}
