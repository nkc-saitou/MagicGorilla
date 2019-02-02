using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetNowAfterScene : SingletonMonoBehaviour<GetNowAfterScene> {

    public string BeforeScene { get; set; }

    public string NowScene { get; set; }

    void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    void OnActiveSceneChanged(Scene prevScene, Scene nextScene)
    {
        BeforeScene = NowScene;
        NowScene = nextScene.name;
    }



    // Update is called once per frame
    void Update () {
		
	}
}
