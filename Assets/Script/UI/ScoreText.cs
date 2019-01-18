using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {
    public Text timerText;
    public Text scoreText;

    ScoreManager sManager;

    void Start()
    {
        if (GameObject.Find("ScoreManager"))
        {
            sManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

            this.ObserveEveryValueChanged(_ => sManager.Score).
                Subscribe(_ => scoreText.text = "Score : " + _.ToString());

            this.ObserveEveryValueChanged(_ => sManager.Timer).
                Subscribe(_ => timerText.text = "Time : " + _.ToString("f2"));
        }
    }
}
