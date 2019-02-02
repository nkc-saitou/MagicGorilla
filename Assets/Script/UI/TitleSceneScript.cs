using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;

public class TitleSceneScript : MonoBehaviour {
    //FadeController fade;

    bool isFade = false;

    string[] resultStr = new string[3];

    void Start () {
        AudioManager.Instance.PlayBGM("Title");

        Debug.Log(GameModeManager.Instance._GameState);

        if(GetNowAfterScene.Instance.NowScene == "Title" && GetNowAfterScene.Instance.BeforeScene == "PlayerTest")
        {
            if (GameModeManager.Instance._GameState == GameState.none)
                GameModeManager.Instance._GameState = GameState.result;
        }

        if (GameModeManager.Instance._GameState == GameState.tutorial) GameModeManager.Instance._GameState = GameState.none;

        StartCoroutine(timeSerif());

        //fade = GameObject.Find("Canvas").GetComponent<FadeController>();
        //fade.IsFadeOut = true;
        //gameObject.UpdateAsObservable().
        //    TakeUntilDestroy(this).
        //    Delay(System.TimeSpan.FromSeconds(1f)).
        //    Where(_ => Input.anyKeyDown).
        //    Take(1).
        //    Subscribe(_ => GameStart());

            //this.UpdateAsObservable()
            //    .TakeUntilDestroy(this)
            //    .Where(_ => PlayerInput.Instance.IsOVRTriggerDown && isFade == false)
            //    .Subscribe(_ =>
            //    {
            //        isFade = true;
            //        EffectManager.Instance.FadeScene("PlayerTest");
            //        GameStart();
            //    });
    }
	
    IEnumerator timeSerif()
    {
        yield return new WaitForSeconds(1.0f);

        if (GameModeManager.Instance._GameState == GameState.result)
        {
            resultStr[0] = "おお、お疲れさま";
            resultStr[1] = "見事な戦いっぷりじゃったぞ";
            resultStr[2] = "今回のお主のタイムは\n" + ScoreManager.Instance.Timer.ToString("f2") + " 秒じゃ";
            SerifManager.Instance.SerifStart(resultStr);

            GameModeManager.Instance._GameState = GameState.none;
        }

    }
    void GameStart()
    {
        if (GameObject.Find("ScoreManager"))
        {
            ScoreManager sManager=GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
            sManager.ReStartValue();
        }
        //SceneChange();
    }

    void SceneChange()
    {
        //fade.IsFadeIn = true;
        //Observable.Timer(System.TimeSpan.FromSeconds(1)).
        //    Subscribe(_ => SceneManager.LoadScene("SampleScene"));
    }
}
