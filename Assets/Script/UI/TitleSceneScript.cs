using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;

public class TitleSceneScript : MonoBehaviour {
    //FadeController fade;

    bool isFade = false;

    void Start () {
        AudioManager.Instance.PlayBGM("Title");
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
