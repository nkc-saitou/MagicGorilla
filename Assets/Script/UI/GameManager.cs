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

    public Text text;

    string sceneName = "ResultScene";

    ScoreManager sManager;
    //FadeController fade;
    //Fader fader;

    void Start()
    {
        sManager = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        //fader = FindObjectOfType<Fader>();
        //fade = GameObject.Find("Canvas").GetComponent<FadeController>();
        //fade.IsFadeOut = true;

        //fader.FadeIn();

        StartCoroutine(StartText());


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

        //動画用///////////////
        //gameObject.UpdateAsObservable().
        //    TakeUntilDestroy(this).
        //    Delay(System.TimeSpan.FromSeconds(1f)).
        //    Where(_ => Input.GetKey(KeyCode.R)).
        //    Take(1).
        //    Subscribe(_ =>
        //    {
        //        sManager.AddScore(150);
        //        IsGameClear = true;
        //        foreach (var ene in GameObject.FindGameObjectsWithTag("Enemy"))
        //        {
        //            ene.GetComponent<BaseEnemy>().EnemyHP -= 1;
        //        }
        //    });

        //gameObject.UpdateAsObservable().
        //    TakeUntilDestroy(this).
        //    Where(_ => Input.GetKeyDown(KeyCode.A)).
        //    Subscribe(_ =>
        //    {
        //        foreach (var ene in GameObject.FindGameObjectsWithTag("Enemy"))
        //        {
        //            ene.GetComponent<BaseEnemy>().EnemyHP -= 1;
        //        }
        //        if (GameObject.FindGameObjectWithTag("Boss"))
        //        {
        //            GameObject.FindGameObjectWithTag("Boss").GetComponent<BaseEnemy>().EnemyHP -= 1;
        //        }
        //    });
        ///////////////////
    }

    IEnumerator StartText()
    {
        yield return new WaitForSeconds(1.5f);
        text.text = "Ready...";
        yield return new WaitForSeconds(1f);
        text.text = "Start!";
        yield return new WaitForSeconds(1f);
        IsStart = true;
        sManager.TimeFlg = true;
        text.enabled = false;
    }

    /// <summary>
    /// クリア処理
    /// </summary>
    void GameClear()
    {
        Debug.Log("Clear");
        text.enabled = true;
        text.text = "クリア";
        sManager.TimeFlg = false;
        Observable.Timer(System.TimeSpan.FromSeconds(3)).
            Subscribe(_ => SceneChange());
    }


    /// <summary>
    /// ゲームオーヴァー処理
    /// </summary>
    void GameOver()
    {
        text.enabled = true;
        text.text = "失敗";
        sManager.TimeFlg = false;
        Observable.Timer(System.TimeSpan.FromSeconds(3)).
            Subscribe(_ => SceneChange());
    }

    void SceneChange()
    {
        //fader.FadeOut();
        //Fader.Instance.gameLogic("result");
        Observable.Timer(System.TimeSpan.FromSeconds(1)).
            Subscribe(_ => SceneManager.LoadScene(sceneName));
    }
}
