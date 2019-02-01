using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine.SceneManagement;

public class SerifManager : SingletonMonoBehaviour<SerifManager> {

    Text serifText;

    Animator anim;

    int rootSerifIndex = 0;

    bool isSerifInterbar = false; //インターバル中かどうか

    float time;

    int serifLength;

    public bool IsSerinSend { get; private set; }

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        serifText = GameObject.FindGameObjectWithTag("ChatSystem").transform.GetChild(0).GetComponent<Text>();
        anim = GameObject.FindGameObjectWithTag("ChatSystem").GetComponent<Animator>();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        serifText = GameObject.FindGameObjectWithTag("ChatSystem").transform.GetChild(0).GetComponent<Text>();
        anim = GameObject.FindGameObjectWithTag("ChatSystem").GetComponent<Animator>();
    }

    public void SerifStart(string[] str)
    {
        if (IsSerinSend != false) return;

        serifLength = str.Length;

        IsSerinSend = true;

        this.ObserveEveryValueChanged(_ => rootSerifIndex)
            .Where(_ => serifLength > rootSerifIndex)
            .Subscribe(_ => serifText.text = str[rootSerifIndex]);

        anim.SetTrigger("IsStartChat");
    }

    private void Update()
    {
        if ((OVRInput.Get(OVRInput.Button.Right) || Input.GetKeyDown(KeyCode.I)) && isSerifInterbar == false)
        {
            rootSerifIndex++;

            isSerifInterbar = true;
        }

        if(isSerifInterbar == true) TimeInterbar();

        if (serifLength == rootSerifIndex && IsSerinSend == true)
        {
            IsSerinSend = false;
            anim.SetTrigger("IsEndChat");
            serifLength = 0;
        }


    }

    void TimeInterbar()
    {
        time += Time.deltaTime;

        if(time >= 0.5f)
        {
            isSerifInterbar = false;
            time = 0.0f;
        }
    }
}
