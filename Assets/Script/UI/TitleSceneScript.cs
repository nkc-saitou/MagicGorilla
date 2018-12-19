using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using UniRx.Triggers;

public class TitleSceneScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.UpdateAsObservable().
            TakeUntilDestroy(this).
            Where(_ => Input.anyKeyDown).
            Subscribe(_ => GameStart());
	}
	
    void GameStart()
    {
        if (GameObject.Find("ScoreManager"))
        {
            ScoreManager sManager=GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
            sManager.ReStartValue();
        }
        SceneManager.LoadScene("SampleScene");
    }
}
