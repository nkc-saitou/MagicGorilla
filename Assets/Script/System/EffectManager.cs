using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.SceneManagement;

using DG.Tweening;

public class EffectManager : SingletonMonoBehaviour<EffectManager> {

    public Renderer fadeImage;

    bool isFade = false;
    float _black;

    [SerializeField] float fadeDuration = 1.0f;

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
                Color col = fadeImage.material.color;
                col.a = _black;
                fadeImage.material.color = col;
            });
    }
}
