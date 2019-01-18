using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;

public class TitleSceneScript : MonoBehaviour {
    FadeController fade;

	void Start () {
        fade = GameObject.Find("Canvas").GetComponent<FadeController>();
        fade.IsFadeOut = true;
        gameObject.UpdateAsObservable().
            TakeUntilDestroy(this).
            Delay(System.TimeSpan.FromSeconds(1f)).
            Where(_ => Input.anyKeyDown).
            Take(1).
            Subscribe(_ => GameStart());
	}
	
    void GameStart()
    {
        if (GameObject.Find("ScoreManager"))
        {
            ScoreManager sManager=GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
            sManager.ReStartValue();
        }
        SceneChange();
    }

    void SceneChange()
    {
        fade.IsFadeIn = true;
        Observable.Timer(System.TimeSpan.FromSeconds(1)).
            Subscribe(_ => SceneManager.LoadScene("SampleScene"));
    }
}
