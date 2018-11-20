using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public Text timerText;
    public Text ScoreText;


    ReactiveProperty<int> score =new ReactiveProperty<int>();
    ReactiveProperty<float> timer = new ReactiveProperty<float>();

    GameManager gameManager;
    float time;

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
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        score.TakeUntilDestroy(this).
            Subscribe(_ =>ScoreText.text="Score : "+_.ToString()).AddTo(this);
        timer.TakeUntilDestroy(this).
            Subscribe(_ => timerText.text ="Time : "+ _.ToString("f2"));

        this.UpdateAsObservable().
            TakeUntilDestroy(this).
            //Where(_=>gameManager.IsStart).
            Where(_ => !gameManager.IsGameClear).
            Subscribe(_ => timer.Value += Time.deltaTime);
	}

    void Score()
    {

    }

    public void AddScore(int plusScore)
    {
        score.Value+=plusScore;
    }
}
