using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    GameManager gameManager;
    public bool TimeFlg { get; set; }

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
        this.UpdateAsObservable().
            TakeUntilDestroy(this).
            Where(_ => TimeFlg).
            Subscribe(_ => Timer += Time.deltaTime);
    }

    public void ReStartValue()
    {
        Score = 0;
        Timer = 0;
    }


    public void AddScore(int plusScore)
    {
        Score += plusScore;
    }
}
