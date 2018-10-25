using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerCore : MonoBehaviour {

    [SerializeField] ObjectCollision objectCollision;

	// Use this for initialization
	void Start () {

        //当たった時の処理
        objectCollision.OnCollision
            .TakeUntilDestroy(this)
            .Where(colObj => colObj.GetComponent<BaseMagic>() == null)
            .Subscribe(collision => AplayDamage());
	}

    /// <summary>
    /// ダメージ処理
    /// </summary>
    void AplayDamage()
    {
        Debug.Log("ダメージ");
    }
}