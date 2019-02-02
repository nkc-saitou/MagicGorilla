﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using UnityEngine.SceneManagement;

public class SerifManager : SingletonMonoBehaviour<SerifManager> {

    Text serifText;

    Animator anim;

    int serifLength; // 渡された文字列配列の長さ

    int rootSerifIndex = 0; //現在クリックした回数

    bool isSerifInterbar = false; //インターバル中かどうか

    float time;

    bool isCheckChat; //チャット中かどうか確かめるためのbool
    List<string> checkStr = new List<string>(); //この文字列が今会話中の内容かどうかを確かめる
    string checkKey;


    public bool IsSerinSend { get; private set; }

    /// <summary>
    /// セリフをストップするかどうか
    /// </summary>
    public bool IsSerifStop { get; set; }

    private void Awake()
    {
        //if (this != Instance)
        //{
        //    Destroy(gameObject);
        //    return;
        //}

        //DontDestroyOnLoad(gameObject);
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

    public void SerifStart(string[] str , string key = "")
    {
        if (IsSerinSend != false) return;

        serifLength = str.Length;

        checkStr.AddRange(str);
        checkKey = key;

        IsSerinSend = true;

        this.ObserveEveryValueChanged(_ => rootSerifIndex)
            .Where(_ => serifLength > rootSerifIndex)
            .Subscribe(_ => serifText.text = str[rootSerifIndex]);

        anim.SetTrigger("IsStartChat");
    }

    /// <summary>
    /// 会話中かどうか
    /// </summary>
    /// <param name="str"></param>
    /// <param name="key"></param>
    /// <returns>会話中だったらtrue</returns>
    public bool IsChat(string[] str,string key)
    {
        if (IsSerinSend != true) return false;

        if (checkStr[0] == str[0] && checkKey == key) return true;
        else return false;
    }

    /// <summary>
    /// 外部からセリフを次に送る
    /// </summary>
    public void NextSerif()
    {
        if (serifLength >= rootSerifIndex) rootSerifIndex++;
    }

    private void Update()
    {
        if ((OVRInput.Get(OVRInput.Button.Right) || Input.GetKeyDown(KeyCode.I)) && isSerifInterbar == false && IsSerifStop == false)
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
            rootSerifIndex = 0;
            checkStr.Clear();
            checkKey = "";
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
