using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class FadeController : MonoBehaviour
{
    float r, g, b, a;
    const float fadeA = 0.02f;        

    bool isFadeOut = false;  
    bool isFadeIn = false;   

    public bool IsFadeIn
    {
        get { return isFadeIn; }
        set
        {
            if (!isFadeOut)
            {
                isFadeIn = value;
            }
        }
    }

    public bool IsFadeOut
    {
        get { return isFadeOut; }
        set
        {
            if (!isFadeIn)
            {
                isFadeOut = value;
            }
        }
    }

    [SerializeField]
    Image image;                

    void Start()
    {
        r = image.color.r;
        g = image.color.g;
        b = image.color.b;
        a = image.color.a;

        this.UpdateAsObservable().
            TakeUntilDestroy(this).
            Subscribe(_ => Fade());

    }

    void Fade()
    {
        if (isFadeIn)
        {
            FadeIn();
        }

        if (isFadeOut)
        {
            FadeOut();
        }
    }

    void FadeIn()
    {
        image.enabled = true;
        a += fadeA;
        AlphaSet();
        if (a >= 1)
        {
            isFadeIn = false;
        }
    }

    void FadeOut()
    {
        image.enabled = true;
        a -= fadeA;
        AlphaSet();
        if (a <= 0)
        {
            isFadeOut = false;
            image.enabled = false;
        }
    }


    void AlphaSet()
    {
        image.color = new Color(r, g, b, a);
    }
}
