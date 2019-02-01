using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;

public class TutorialGaid : BaseGaid {

    protected override void DoStart()
    {

    }

    protected override void DoUpdate()
    {

    }

    public override void GaidAction()
    {
        if(GameModeManager.Instance._GameState == GameState.None)
        {
            string[] str = new string[1];

            str[0] = "なんじゃ？魔法の打ち方を教えてほしいのかの？";
            SerifManager.Instance.SerifStart(str);
        }
    }
}
