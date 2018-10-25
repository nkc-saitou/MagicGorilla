using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Linq;

public enum MagicType
{
    _fire,
    _ice,
    _lightning,
    _rock,
    _sword,
    _none
}

[RequireComponent(typeof(MagicFire))]
[RequireComponent(typeof(MagicIce))]
[RequireComponent(typeof(MagicLightning))]
[RequireComponent(typeof(MagicRock))]
[RequireComponent(typeof(MagicSword))]
public class Shot : MonoBehaviour {

    [Header("火、氷、雷、石、剣")]
    [SerializeField] GameObject[] magicEffect;

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

    void Start ()
    {
        actionLis.AddRange(FindObjectInterface<IAction>());

        memoryState = MagicType._none;

        //毎フレームInputStateを監視
        this.UpdateAsObservable()
            .Subscribe(_ => state = GetComponent<PlayerInput>()._InputState);

        //InputStateに変化があった場合、MagicShotを実行
        gameObject.ObserveEveryValueChanged(_ => state)
            .Subscribe(_ => MagicShot());

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
            case InputState._accel:    MagicShot(memoryState);          break;
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
        actionLis[(int)type].Charge();
        memoryState = type;

    }

    /// <summary>
    /// 魔法を打つ
    /// </summary>
    /// <param name="type"></param>
    void MagicShot(MagicType type)
    {
        if (memoryState == MagicType._none) return;
        actionLis[(int)type].Shot(Enemy);

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

    /// <summary>
    /// 投げることが出来る(IActionインターフェイスが実装されている)ものを取得する
    /// </summary>
    /// <typeparam name="T">InterFaceの型</typeparam>
    /// <returns>InterFaceが実装されているコンポーネントのList</returns>
    List<T> FindObjectInterface<T>() where T : class
    {
        List<T> tempLis = new List<T>();
        foreach (var n in FindObjectsOfType<Component>())
        {
            var component = n as T;
            if (component != null)
            {
                tempLis.Add(component);
            }
        }
        return tempLis;
    }
}
