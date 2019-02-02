using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;

public class OutSideGaid : BaseGaid {

    protected override void DoStart()
    {

    }

    protected override void DoUpdate()
    {

    }

    public override void GaidAction()
    {
        if (GameModeManager.Instance._GameState == GameState.none)
        {
            EffectManager.Instance.FadeScene("PlayerTest");
        }
        else if(GameModeManager.Instance._GameState == GameState.tutorial)
        {
            string[] str = new string[2];
            str[0] = "こら！まだわしの話は終わっておらんぞ！";
            str[1] = "こっちに戻ってくるのじゃ！！";
            SerifManager.Instance.SerifStart(str);
        }
    }
}
