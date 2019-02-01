//=======================================================
// 今のゲームステートを管理
//=======================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    tutorial = 0,
    Game,
    None
}

public class GameModeManager : SingletonMonoBehaviour<GameModeManager> {

    public GameState _GameState { get; set; }

}
