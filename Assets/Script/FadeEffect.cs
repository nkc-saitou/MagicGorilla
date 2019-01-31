using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

using DG.Tweening;

public class FadeEffect : SingletonMonoBehaviour<FadeEffect> {

    Subject<Unit> instantiateSubject = new Subject<Unit>();

    public IObservable<Unit> OnInstantiate
    {
        get { return instantiateSubject; }
    }

    public Renderer fadeImage;

    Transform cam;

    bool isFade;
    float _black;

    [SerializeField] float fadeDuration;

    void Awake()
    {
        //インスタンスがない場合
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

        cam = Camera.main.transform;

        instantiateSubject.OnNext(Unit.Default);

    }

    void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        cam = Camera.main.transform;

        instantiateSubject.OnNext(Unit.Default);
    }

    public void FadeScene(string sceneName)
    {
        UpdateFadeImage();
        StartCoroutine(FadeControl(sceneName));
    }

    IEnumerator FadeControl(string sceneName)
    {
        if (isFade == true) yield return null;

        yield return FadeOut();

        SceneManager.LoadScene(sceneName);

        yield return FadeIn();
    }

    IEnumerator FadeOut()
    {
        isFade = true;

        DOTween.To(black => _black = black, 0.0f, 1.0f, fadeDuration)
            .OnComplete(() => isFade = false);

        yield return new WaitWhile(() => isFade == true);
    }

    IEnumerator FadeIn()
    {
        isFade = true;

        DOTween.To(black => _black = black, 1.0f, 0.0f, fadeDuration)
            .OnComplete(() => isFade = false);

        yield return new WaitWhile(() => isFade == true);
    }

    void UpdateFadeImage()
    {
        this.UpdateAsObservable()
            .Where(_ => isFade)
            .Subscribe(_ => 
            {
                Debug.Log(_black);
                Color col = fadeImage.material.color;
                col.a = _black;
                fadeImage.material.color = col;
            });
    }
}
