using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class ResultScript : MonoBehaviour {
    [SerializeField, Tooltip("スコア用")]
    Text scoreText;

    [SerializeField, Tooltip("タイム用")]
    Text timeText;

    ScoreManager scoreManager;
    FadeController fade;
	
	void Start () {
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        fade = GameObject.Find("Canvas").GetComponent<FadeController>();
        fade.IsFadeOut = true;
        scoreText.text = "スコア : " + scoreManager.Score.ToString();
        timeText.text = "タイム : " + scoreManager.Timer.ToString("f2");

        gameObject.UpdateAsObservable().
            TakeUntilDestroy(this).
            Delay(System.TimeSpan.FromSeconds(1f)).
            Where(_ => Input.anyKeyDown).
            Take(1).
            Subscribe(_ => SceneChange());
    }
	

    void SceneChange()
    {
        fade.IsFadeIn = true;
        Observable.Timer(System.TimeSpan.FromSeconds(1)).
            Subscribe(_ => SceneManager.LoadScene("Title"));
    }
}
