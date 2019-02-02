using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;

public class TutorialGaid : BaseGaid {

    string[] noneSerif = new string[6];
    string[] t_1 = new string[2];
    string[] t_2 = new string[5];

    bool isFirst = false;

    protected override void DoStart()
    {

    }

    protected override void DoUpdate()
    {

        Tutorial_1_Action();
    }

    void Tutorial_1_Action()
    {
        //if (SerifManager.Instance.IsChat(noneSerif, "noneSerif"))
        //{
        //    if ((OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad) || Input.GetKeyDown(KeyCode.P)) && isFirst == true)
        //    {
        //        isFirst = false;
        //        GameModeManager.Instance.ChangeGameState(GameState.tutorial);
        //        Debug.Log(GameModeManager.Instance._GameState);
        //        noneSerif[1] = "そうかそうか！よかろう！わしがしっかりと教えてやる";
        //        noneSerif[2] = "ではまずは、奥の本の所でジェスチャーを登録してくるのじゃ";
        //        SerifManager.Instance.IsSerifStop = false;
        //        SerifManager.Instance.NextSerif();
        //        //SpriteActive(0, 2, false, 0.0f);
        //        sprite.SetActive(false);
        //    }
        //    else if ((OVRInput.GetDown(OVRInput.Button.Two) || Input.GetKeyDown(KeyCode.L)) && isFirst == true)
        //    {
        //        isFirst = false;
        //        noneSerif[1] = "そうか…違うのか…";
        //        noneSerif[2] = "つまらぬのう…";
        //        SerifManager.Instance.IsSerifStop = false;
        //        SerifManager.Instance.NextSerif();
        //        //SpriteActive(0, 2, false, 0.0f);
        //        sprite.SetActive(false);
        //    }
        //}

        if (SerifManager.Instance.IsSerifNumber(t_2, "t_2", 4) && isFirst == false)
        {
            isFirst = true;
            EffectManager.Instance.FadeScene("Tutorial");
        }
    }

    public override void GaidAction()
    {
        switch(GameModeManager.Instance._GameState)
        {
            case GameState.none:
                GameModeManager.Instance.ChangeGameState(GameState.tutorial);

                noneSerif[0] = "おお、よく来たのう！ゆっくりしていけ";
                noneSerif[1] = "…ん…？なんじゃ？";
                noneSerif[2] = "もしや、WASHI流の魔法を教えてほしいのか？";
                noneSerif[3] = "ふむ、よかろう！わしがしっかりと鍛えてやる";
                noneSerif[4] = "ではまずは、奥の本の所でジェスチャーを登録してくるのじゃ";
                noneSerif[5] = "終わったらわしにもう一回話しかけるのじゃぞ";
                SerifManager.Instance.SerifStart(noneSerif, "noneSerif");

                break;

            case GameState.tutorial:
                t_1[0] = "何をぼさっとしておる";
                t_1[1] = "はやく奥の本の所でジェスチャーを登録してくるのじゃ";
                SerifManager.Instance.SerifStart(t_1, "t_1");
                break;

            case GameState.tutorial_2:
                t_2[0] = "ふむ、登録もできたようじゃの";
                t_2[1] = "ではさっそくじゃが、説明にはいるとするかの";
                t_2[2] = "習うより慣れろ。実際に体験してもらった方が早いかのう";
                t_2[3] = "準備するからちと待っておれ";
                t_2[4] = "……………";
                SerifManager.Instance.SerifStart(t_2, "t_2");
                break;
        }
    }

    //public void SpriteActive(int startIndex, int endIndex, bool isActive, float dlay = 1.0f)
    //{
    //    StartCoroutine(Active(startIndex, endIndex, isActive, dlay));
    //}

    //float t = 0;

    //IEnumerator Active(int startIndex, int endIndex, bool isActive, float dlay)
    //{
    //    yield return new WaitWhile(() => isWaitTime(dlay));

    //    Debug.Log("oo");
    //    for (int i = startIndex; i < endIndex; i++)
    //    {
    //        sprite[i].SetActive(isActive);
    //    }
    //    yield return null;
    //}

    //bool isWaitTime(float dlay)
    //{
    //    t += Time.deltaTime;

    //    if (t >= dlay)
    //    {
    //        t = 0;
    //        return false;
    //    }
    //    else return true;
    //}
}
