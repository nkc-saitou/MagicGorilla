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

	
	void Start () {
        scoreManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        scoreText.text = "スコア : " + scoreManager.Score;
        timeText.text = "タイム : " + scoreManager.Timer;

        gameObject.UpdateAsObservable().
            TakeUntilDestroy(this).
            Where(_ => Input.anyKeyDown).
            Subscribe(_ => SceneChange());
    }
	

    void SceneChange()
    {
        SceneManager.LoadScene("Title");
    }
}
