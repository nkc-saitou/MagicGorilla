using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;

public class TutorialGaid : BaseGaid {

    public GameObject[] sprite;

    string[] noneSerif = new string[3];
    string[] t_1 = new string[2];

    protected override void DoStart()
    {

    }

    protected override void DoUpdate()
    {

        Tutorial_1_Action();
    }

    void Tutorial_1_Action()
    {
        if (SerifManager.Instance.IsChat(noneSerif, "noneSerif"))
        {
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger) || Input.GetKeyDown(KeyCode.P))
            {
                GameModeManager.Instance.ChangeGameState(GameState.tutorial);
                Debug.Log(GameModeManager.Instance._GameState);
                noneSerif[1] = "そうかそうか！よかろう！わしがしっかりと教えてやる";
                noneSerif[2] = "ではまずは、奥の本の所でジェスチャーを登録してくるのじゃ";
                SerifManager.Instance.IsSerifStop = false;
                SerifManager.Instance.NextSerif();
                SpriteActive(0, 1, false, 0.0f);
            }
            else if (OVRInput.GetDown(OVRInput.Button.Two) || Input.GetKeyDown(KeyCode.L))
            {
                noneSerif[1] = "そうか…違うのか…";
                noneSerif[2] = "つまらぬのう…";
                SerifManager.Instance.IsSerifStop = false;
                SerifManager.Instance.NextSerif();
                SpriteActive(0, 1, false, 0.0f);
            }
        }
    }

    public override void GaidAction()
    {
        switch(GameModeManager.Instance._GameState)
        {
            case GameState.none:
                noneSerif[0] = "なんじゃ？魔法の打ち方を教えてほしいのか？";
                SerifManager.Instance.SerifStart(noneSerif, "noneSerif");

                SpriteActive(0, 1, true);

                SerifManager.Instance.IsSerifStop = true;
                break;

            case GameState.tutorial:
                t_1[0] = "何をぼさっとしておる";
                t_1[1] = "はやく奥の本の所でジェスチャーを登録してくるのじゃ";
                SerifManager.Instance.SerifStart(t_1, "t_1");
                break;
        }
    }

    public void SpriteActive(int startIndex,int endIndex,bool isActive,float dlay = 1.0f)
    {
        StartCoroutine(Active(startIndex, endIndex + 1, isActive,dlay));
    }

    IEnumerator Active(int startIndex, int endIndex, bool isActive , float dlay)
    {
        yield return new WaitForSeconds(dlay);

        for (int i = startIndex; i < endIndex; i++)
        {
            sprite[i].SetActive(isActive);
        }
    }
}
