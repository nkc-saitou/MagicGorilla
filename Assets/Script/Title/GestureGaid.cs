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

    bool isRockEnd = false;

    bool isSettingEnd = false;

    bool isStart = true;

    bool isAnimationEnd = true;

    int gestureNum = 0;

    protected override void DoStart()
    {
        HandGestureImageObj[0].SetActive(true);
        HandGestureImageObj[1].SetActive(false);
    }

    protected override void DoUpdate()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || Input.GetMouseButtonDown(0) && isStart)
        {
            isStart = false;
            setting.SetTargetPress();
            gestureNum++;
        }

        if (gestureNum == 1 && handImage[0].fillAmount >= 1 && isRockEnd == false)
        {
            isRockEnd = true;
            HandGestureImageObj[0].SetActive(false);
            HandGestureImageObj[1].SetActive(true);
        }

        if ((OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || Input.GetMouseButtonDown(0)) && isRockEnd == true)
        {
            isRockEnd = false;
            gestureNum++;
            setting.SetNonTargetPress();
        }

        if (gestureNum == 2 && handImage[1].fillAmount >= 1)
        {
            Debug.Log("ok");
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
            StartCoroutine(EndWaitTime());
            manager.ObjectActive();
        }

    }

    IEnumerator EndWaitTime()
    {
        yield return new WaitForSeconds(1.0f);
        GestureSettingBord.SetActive(false);
        gestureNum = 0;
        isAnimationEnd = true;
        isSettingEnd = false;

    }

    public override void GaidAction()
    {
        if (isAnimationEnd != true) return;

        Debug.Log("はじまったよー");

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
