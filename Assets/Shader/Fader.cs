using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using DG.Tweening;

public class Fader : SingletonMonoBehaviour<Fader> {

    public Material material;

    private bool isFade = false;
    private float _black;

    [SerializeField]
    private float fadeDuration = 1.0f;

    void Awake()
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

        StartCoroutine(gameLogic());

    }


    private void Update()
    {

    }

    public IEnumerator gameLogic()
    {
        yield return FadeOut();

        Debug.Log("Done FadeOut");

        yield return FadeIn();

        Debug.Log("Done FadeIn");

    }

    public IEnumerator FadeOut()
    {
        isFade = true;

        DOTween.To(black => _black = black, 1.0f, 0.0f, fadeDuration)
            .OnComplete(() => {
                isFade = false;
            });

        yield return new WaitWhile(() => isFade == true);

    }

    public IEnumerator FadeIn()
    {
        isFade = true;

        DOTween.To(black => _black = black, 0.0f, 1.0f, fadeDuration)
            .OnComplete(() => {
                isFade = false;
            });

        yield return new WaitWhile(() => isFade == true);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (isFade)
        {
            UpdateMaterial();
            Graphics.Blit(source, destination, material);
        }
    }

    void UpdateMaterial()
    {
        Debug.Log(_black);
        material.SetFloat("black", _black);
    }
}
