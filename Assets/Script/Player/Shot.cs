using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Linq;
using System;

public class Shot : MonoBehaviour {

    //-----------------------------------------------------
    // private
    //-----------------------------------------------------

    List<IAction> actionLis = new List<IAction>();

    InputState state;

    MagicType memoryState;

    MagicType type;

    //-----------------------------------------------------
    // public
    //-----------------------------------------------------

    public GameObject Enemy { get; private set; }

    public Transform pos;

    void Start ()
    {
        actionLis.AddRange(MagicProvider.ActionLis);

        memoryState = MagicType._none;

        //毎フレームInputStateを監視
        this.UpdateAsObservable()
            .Subscribe(_ => state = PlayerInput.Instance._InputState);

        //InputStateに変化があった場合、MagicShotを実行
        this.UpdateAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(1.5f))
            .Subscribe(_ => MagicShot());

        this.UpdateAsObservable()
            .Where(_ => state != InputState._none)
            .Subscribe(_ =>
            {
                if(state == InputState._accel) MagicShot(memoryState);
            });

        this.UpdateAsObservable()
            .Where(_ => Enemy == null)
            .Where(_ => SetAttackTarget() != null)
            .Subscribe(_ => Enemy = SetAttackTarget());

    }

    void MagicShot()
    {
        MagicType tempType = MagicType._none;

        switch(state)
        {
            case InputState._armRight: tempType = MagicType._fire;      break;
            case InputState._armLeft:  tempType = MagicType._ice;       break;
            case InputState._armUp:    tempType = MagicType._lightning; break;
            case InputState._armDown:  tempType = MagicType._rock;      break;
            case InputState._armFront: tempType = MagicType._sword;     break;
        }

        if (tempType == MagicType._none) return;
        MagicSet(tempType);
    }

    /// <summary>
    /// 打つ魔法をセットする
    /// </summary>
    /// <param name="type"></param>
    void MagicSet(MagicType type)
    {
        if (memoryState != MagicType._none) return;
        actionLis[(int)type].Charge(pos);
        memoryState = type;
    }

    /// <summary>
    /// 魔法を打つ
    /// </summary>
    /// <param name="type"></param>
    void MagicShot(MagicType type)
    {
        if (memoryState == MagicType._none) return;
        actionLis[(int)type].Shot(pos.gameObject);

        memoryState = MagicType._none;
    }

    /// <summary>
    /// 攻撃する敵を設定
    /// </summary>
    GameObject SetAttackTarget()
    {
        //敵を格納するlist
        List<GameObject> tempList = new List<GameObject>();

        foreach (BaseEnemy enemyPos in FindObjectsOfType<BaseEnemy>())
        {
            GameObject tempObj = enemyPos.gameObject;

            tempList.Add(tempObj);
        }

        //敵がいなかったら処理を終了
        if (tempList.Count <= 0) return null;

        //距離が近い順に並べ替え
        List<GameObject> enemyPosLis = new List<GameObject>();
        enemyPosLis = tempList.OrderBy(pos => Vector3.Distance(pos.gameObject.transform.position, transform.position)).ToList();

        return enemyPosLis[0];
    }
}
