using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public Text timerText;
    public Text ScoreText;

    GameManager gameManager;
    float time;

    public int Score { get; set; }

    public float Timer { get; set; }


    #region singleton
    public static ScoreManager Instance
    {
        get; private set;
    }



    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    void Start () {
        Score = 0;
        Timer = 0;
        if (GameObject.Find("GameManager"))
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        

        this.ObserveEveryValueChanged(_=>Score).
            Where(_=>ScoreText).
            Subscribe(_ =>ScoreText.text="Score : "+_.ToString());

        this.ObserveEveryValueChanged(_=>Timer).
            Where(_=>timerText).
            Subscribe(_ => timerText.text ="Time : "+ _.ToString("f2"));

        this.UpdateAsObservable().
            TakeUntilDestroy(this).
            Where(_ => gameManager&&!gameManager.IsGameClear).
            Subscribe(_ => Timer += Time.deltaTime);
    }

    public void Reset()
    {
        Score = 0;
        Timer = 0;
    }


    public void AddScore(int plusScore)
    {
        Score += plusScore;
    }
}
