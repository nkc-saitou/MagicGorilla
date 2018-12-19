using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class AttackAllocation : MonoBehaviour {
    [SerializeField, Tooltip("攻撃位置")]
    Vector3[] attackPos=new Vector3[5];
    bool[] attackPosFlg = new bool[5];
    GameObject[] objList = new GameObject[5];


    void Start () {
        this.UpdateAsObservable().
            TakeUntilDestroy(this).
            Subscribe(_ => ObjCheck());
    }

    /// <summary>
    /// 移動場所割り振り
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public Vector3 PositionCheck(GameObject obj)
    {
        for(int i = 0; i < attackPosFlg.Length; i++)
        {
            if (objList[i]==obj)
            { 
                return attackPos[i];
            }
        }
        for (int i = 0; i < attackPosFlg.Length; i++)
        {
            if (!attackPosFlg[i])
            {
                attackPosFlg[i] = true;
                objList[i] = obj;
                return attackPos[i];
            }
        }
        return Vector3.zero;
    }


    void ObjCheck()
    {
        for (int i = 0; i < objList.Length; i++)
        {
            if (!objList[i])
            {
                attackPosFlg[i] = false;
            }
        }
    }
}
