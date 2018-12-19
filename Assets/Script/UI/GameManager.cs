using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class GameManager : MonoBehaviour {
    BaseEnemy bossBase;

    public bool IsStart;
    public bool IsGameClear;
    public bool IsGameOver;

    public Text clearText;

    string sceneName = "ResultScene";

    ScoreManager sManager;


    void Start()
    {
        clearText.enabled = false;
        sManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();

        gameObject.UpdateAsObservable().
            Where(_ => GameObject.FindGameObjectWithTag("Boss")).
            Take(1).
            Subscribe(_ =>
            {
                bossBase = GameObject.FindGameObjectWithTag("Boss").GetComponent<BaseEnemy>();
                gameObject.ObserveEveryValueChanged(__ => bossBase.EnemyHP).
                    Where(__ => __ <= 0).
                    Subscribe(__ => IsGameClear = true);
            });



        gameObject.ObserveEveryValueChanged(_ => IsGameClear).
            Where(_ => _).
            Take(1).
            Subscribe(_ => GameClear());

        gameObject.ObserveEveryValueChanged(_ => IsGameOver).
            Where(_ => _).
            Take(1).
            Subscribe(_ => GameOver());
    }

    /// <summary>
    /// クリア処理
    /// </summary>
    void GameClear()
    {
        Debug.Log("Clear");
        clearText.enabled = true;
        sManager.TimeFlg = false;
        Observable.Timer(System.TimeSpan.FromSeconds(3)).
            Subscribe(_ => SceneManager.LoadScene(sceneName));

    }

    /// <summary>
    /// ゲームオーヴァー処理
    /// </summary>
    void GameOver()
    {
        sManager.TimeFlg = false;

    }
}
