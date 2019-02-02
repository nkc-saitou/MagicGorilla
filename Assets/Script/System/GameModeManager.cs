//=======================================================
// 今のゲームステートを管理
//=======================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    none = 0,
    tutorial,
    tutorial_2,
    game,
    result
}

public class GameModeManager : SingletonMonoBehaviour<GameModeManager> {

    public GameState _GameState { get; set; }

    public void ChangeGameState(GameState state)
    {
        _GameState = state;
    }

}
