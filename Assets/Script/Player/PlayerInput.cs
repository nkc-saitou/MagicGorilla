using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FVRlib;
using UniRx;
using UniRx.Triggers;
using System;
using UnityEngine.UI;

public enum FVRInputState
{
    none = 0,
    armUp,
    armDown,
    armLeft,
    armRight,
    armFront,
    handUp,
    handDown,
    //accel,
}

public enum GestureInputState
{
    gestureRock,
    gesturePaper
}

//public enum OVRInputState
//{
//    ovrGetUpTrigger = 0,
//    ovrGetDownTrigger,
//    ovrGetDownTouchpad,
//    ovrGetUpTouchpad,
//}

public class PlayerInput : SingletonMonoBehaviour<PlayerInput>
{
    //------------------------------------
    // public
    //------------------------------------

    public IObservable<Unit> OnAccel
    {
        get { return accelSubject; }
    }

    public GameObject pointer;

    //------------------------------------
    // private
    //------------------------------------

    [SerializeField]
    private Transform _LeftHandAnchor;

    [SerializeField]
    private float _MaxDistance = 100.0f;

    FVRConnection fvr;

    Subject<Unit> accelSubject = new Subject<Unit>();



    bool isInterbar = false; //　インターバル中かどうか

    //------------------------------------
    // プロパティ
    //------------------------------------
    public FVRInputState _FVRInputState { get; private set; }

    //public OVRInputState _OVRInputState { get; private set; }

    public GestureInputState _GestureInputState { get; private set; }

    public Vector3 RayHitPos { get; private set; }

    public GameObject HitGameObject { get; set; }

    public GameObject OVRRayHitGameObject { get; private set; }

    public FVRGesture _FVRGesture { get; set; }

    public bool IsRockGesture { get; set; }

    public bool IsOVRTochPadGetDown { get; set; }

    public bool IsOVRTriggerDown { get; set; }

    public bool IsFVRAccel { get; set; }

    //------------------------------------
    // 関数
    //------------------------------------

    void Awake()
    {
        if(this != Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        fvr = FindObjectOfType(typeof(FVRConnection)) as FVRConnection;

        InputObserver();
    }

    private void Update()
    {
        RayHit();

        if(_FVRGesture != null) IsRockGesture = _FVRGesture.held;
    }

    void RayHit()
    {

#if UNITY_EDITOR
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            HitGameObject = hit.transform.gameObject;
        }
#else
        Ray pointerRay = new Ray(_LeftHandAnchor.transform.position, pointer.transform.position);

        RaycastHit hitInfo;
        if (Physics.Raycast(pointerRay, out hitInfo, _MaxDistance))
        {
            HitGameObject = hitInfo.transform.gameObject;
        }
#endif
    }

    ///// <summary>
    ///// 視点入力
    ///// </summary>
    //void IsViewPointInput()
    //{
    //    Vector3 rayOrigin;
    //    RaycastHit hit;

    //    rayOrigin = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.5f));

    //    if (Physics.Raycast(rayOrigin, Camera.main.transform.forward, out hit, Mathf.Infinity) == false)
    //    {
    //        HitGameObject = null;
    //        return;
    //    }

    //    RayHitPos = hit.point;
    //    HitGameObject = hit.transform.gameObject;
    //}

    /// <summary>
    /// エフェクトを一度消した時に呼ぶ
    /// </summary>
    public void InitFVRInputState()
    {
        _FVRInputState = FVRInputState.armFront;
    }

    /// <summary>
    /// Inputの入力状態を監視
    /// </summary>
    void InputObserver()
    {
        //this.UpdateAsObservable()
        //    .Subscribe(_ => IsViewPointInput());

#if UNITY_EDITOR

        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetKeyDown(KeyCode.A))
            .Subscribe(_ => _FVRInputState = FVRInputState.armLeft);

        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetKeyDown(KeyCode.S))
            .Subscribe(_ => _FVRInputState = FVRInputState.armFront);

        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetKeyDown(KeyCode.D))
            .Subscribe(_ => _FVRInputState = FVRInputState.armRight);

        this.UpdateAsObservable()
             .TakeUntilDestroy(this)
            .Where(_ => Input.GetKeyDown(KeyCode.S))
            .Subscribe(_ => _FVRInputState = FVRInputState.armDown);

        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetKeyDown(KeyCode.W))
            .Subscribe(_ => _FVRInputState = FVRInputState.armUp);

        //this.UpdateAsObservable()
        //    .TakeUntilDestroy(this)
        //    .Subscribe(_ => 
        //    {
        //        if (Input.GetKeyDown(KeyCode.F)) IsFVRAccel = true;
        //        else IsFVRAccel = false;

        //    });

        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetKeyDown(KeyCode.F))
            .Subscribe(_ =>
            {
                accelSubject.OnNext(_);
            });

        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetKeyDown(KeyCode.R))
            .Subscribe(_ => _GestureInputState = GestureInputState.gestureRock);

        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetKeyDown(KeyCode.P))
            .Subscribe(_ => _GestureInputState = GestureInputState.gesturePaper);

        //Oculusコントローラーのトリガーを押したとき
        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetKeyDown(KeyCode.C))
            .Subscribe(_ => IsOVRTriggerDown = true);

        //Oculusコントローラーのトリガーを離したとき
        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetKeyDown(KeyCode.V))
            .Subscribe(_ => IsOVRTriggerDown = false);

        //Oculusコントローラーのタッチパッドをクリックしたとき
        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetKeyDown(KeyCode.T))
            .Subscribe(_ => IsOVRTochPadGetDown = true);

        //Oculusコントローラーのタッチパッドをクリックしたとき
        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => Input.GetKeyDown(KeyCode.Y))
            .Subscribe(_ => IsOVRTochPadGetDown = false);

#else

        //一定速度以上で腕を動かしたとき
        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Where(_ => fvr.accel.magnitude > 1.0f)
            .Subscribe(_ =>
            {
                accelSubject.OnNext(_);
            });

        //腕を下に向けているとき
        this.ObserveEveryValueChanged(playerInput => playerInput.fvr.palmOrientation)
            .TakeUntilDestroy(this)
            .Where(_ => fvr.horizontalOrientation == FVRConnection.HorizontalOrientation.front &&
                        fvr.palmOrientation == FVRConnection.PalmOrientation.down)
            .Subscribe(_ => _FVRInputState = FVRInputState.armFront);

        //腕を左に向けているとき
        this.ObserveEveryValueChanged(playerInput => playerInput.fvr.horizontalOrientation)
            .TakeUntilDestroy(this)
            .Where(_ => fvr.horizontalOrientation == FVRConnection.HorizontalOrientation.left)
            .Subscribe(_ => _FVRInputState = FVRInputState.armLeft);

        //腕を右に向けているとき
        this.ObserveEveryValueChanged(playerInput => playerInput.fvr.horizontalOrientation)
            .TakeUntilDestroy(this)
            .Where(_ => fvr.horizontalOrientation == FVRConnection.HorizontalOrientation.right)
            .Subscribe(_ => _FVRInputState = FVRInputState.armRight);

        //腕を上に向けているとき
        this.ObserveEveryValueChanged(playerInput => playerInput.fvr.verticalOrientation)
            .TakeUntilDestroy(this)
            .Where(_ => fvr.verticalOrientation == FVRConnection.VerticalOrientation.up)
            .Subscribe(_ => _FVRInputState = FVRInputState.armUp);

        //腕を下に向けているとき
        this.ObserveEveryValueChanged(playerInput => fvr.verticalOrientation)
            .TakeUntilDestroy(this)
            .Where(_ => fvr.verticalOrientation == FVRConnection.VerticalOrientation.down)
            .Subscribe(_ => _FVRInputState = FVRInputState.armDown);

        //グーのとき
        this.ObserveEveryValueChanged(playerInput => fvr.verticalOrientation)
            .TakeUntilDestroy(this)
            .Where(_ => IsRockGesture == true)
            .Subscribe(_ => _GestureInputState = GestureInputState.gestureRock);

        //パーのとき
        this.ObserveEveryValueChanged(playerInput => fvr.verticalOrientation)
            .TakeUntilDestroy(this)
            .Where(_ => IsRockGesture == false)
            .Subscribe(_ => _GestureInputState = GestureInputState.gesturePaper);

        this.UpdateAsObservable()
            .TakeUntilDestroy(this)
            .Subscribe(_ => 
            {
                IsOVRTochPadGetDown = OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad);
                IsOVRTriggerDown = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);
            });

        ////Oculusコントローラーのトリガーを押したとき
        //this.UpdateAsObservable()
        //    .TakeUntilDestroy(this)
        //    .Where(_ => OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        //    .Subscribe(_ => _OVRInputState = OVRInputState.ovrGetDownTrigger);

        ////Oculusコントローラーのトリガーを離したとき
        //this.UpdateAsObservable()
        //    .TakeUntilDestroy(this)
        //    .Where(_ => OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        //    .Subscribe(_ => _OVRInputState = OVRInputState.ovrGetUpTrigger);

        ////Oculusコントローラーのトリガーを離したとき
        //this.UpdateAsObservable()
        //    .TakeUntilDestroy(this)
        //    .Where(_ => OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad))
        //    .Subscribe(_ => _OVRInputState = OVRInputState.ovrGetDownTouchpad);

        ////Oculusコントローラーのトリガーを離したとき
        //this.UpdateAsObservable()
        //    .TakeUntilDestroy(this)
        //    .Where(_ => OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad) == false)
        //    .Subscribe(_ => _OVRInputState = OVRInputState.ovrGetUpTouchpad);

#endif
    }
}


