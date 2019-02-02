using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;
using UnityEngine.UI;

public class GestureGaid : BaseGaid {

    [Header("イメージ全体のオブジェクト")]
    public GameObject GestureSettingBord;

    [Header("キャリブレーションで時間はかる方のイメージ")]
    public GameObject[] HandGestureImageObj;

    [Header("キャリブレーションで時間はかる方のイメージ")]
    public Image[] handImage;

    [Header("キャリブレーション中のオブジェクトまとめ")]
    public GameObject calibController;

    [Header("完了、確認のオブジェクトまとめ")]
    public GameObject doneController;

    [Header("ジェスチャーチェック用のImage")]
    public Image checkImage;

    [Header("ジェスチャーチェック用のSprite")]
    public Sprite[] checkSp;

    public GestureSetting setting;

    public GaidManager manager;

    int rockCount;

    bool isRockEnd = false;

    bool isPaperEnd = false;

    bool RedyPaperStart = false;

    bool isSettingEnd = false;

    bool isStart = true;

    bool isAnimationEnd = true;

    int gestureNum = 0;

    bool isEnd = false;

    protected override void DoStart()
    {
        HandGestureImageObj[0].SetActive(true);
        HandGestureImageObj[1].SetActive(false);

        End();
    }

    void End()
    {
        setting.OnEndCalib
            .Where(name => name == "rock")
            .Subscribe(name =>
            {
                Debug.Log(gestureNum);
                //setting.SetTargetPress();
                isRockEnd = true;
            });

        setting.OnEndCalib
            .Where(name => name == "paper")
            .Subscribe(name =>
            {
                isPaperEnd = true;
            });
    }

    protected override void DoUpdate()
    {
        if ((OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad) || Input.GetMouseButtonDown(1))&& isStart)
        {
            isStart = false;
            setting.SetTargetPress();
            gestureNum++;
        }

        if (gestureNum == 1 &&  isRockEnd == true)
        {
            HandGestureImageObj[0].SetActive(false);
            HandGestureImageObj[1].SetActive(true);
        }

        if ((OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad) || Input.GetMouseButtonDown(1)) && isRockEnd == true)
        {
            isRockEnd = false;
            gestureNum++;
            setting.SetNonTargetPress();
        }

        if (gestureNum == 2 && isPaperEnd == true)
        {
            isPaperEnd = false;
            calibController.SetActive(false);
            doneController.SetActive(true);

            isSettingEnd = true;
        }

        if (isSettingEnd == false) return;

        if (PlayerInput.Instance._GestureInputState == GestureInputState.gestureRock) checkImage.sprite = checkSp[0];
        else checkImage.sprite = checkSp[1];

        if(OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || Input.GetMouseButtonDown(0))
        {
            GestureSettingBord.GetComponent<Animator>().SetBool("IsStartAnim",false);
            isEnd = true;
        }

        WaitTime();
    }

    float t = 0;

    void WaitTime()
    {
        
        t += Time.deltaTime;

        if(t >= 1.0f && isEnd)
        {
            manager.ObjectActive();
            GestureSettingBord.SetActive(false);
            gestureNum = 0;
            isAnimationEnd = true;
            isSettingEnd = false;
            isEnd = false;
            t = 0;
            if (GameModeManager.Instance._GameState == GameState.tutorial) GameModeManager.Instance._GameState = GameState.tutorial_2;
        }
    }

    //IEnumerator EndWaitTime()
    //{
    //    yield return new WaitForSeconds(1.0f);
    //    GestureSettingBord.SetActive(false);
    //    gestureNum = 0;
    //    isAnimationEnd = true;
    //    isSettingEnd = false;

    //}

    public override void GaidAction()
    {
        if (isAnimationEnd != true) return;

        Debug.Log("はじまったよー");

        setting.ResetCalibrationPress();

        GestureSettingBord.SetActive(true);
        GestureSettingBord.GetComponent<Animator>().SetBool("IsStartAnim",true);
        gestureNum = 0;

        manager.ObjectNonActive();

        calibController.SetActive(true);
        doneController.SetActive(false);

        HandGestureImageObj[0].SetActive(true);
        HandGestureImageObj[1].SetActive(false);

        isStart = true;

        isAnimationEnd = false;

    }
}
