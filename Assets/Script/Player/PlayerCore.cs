using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerCore : MonoBehaviour {

    public ObjectCollision objectCollision;

	// Use this for initialization
	void Start () {

        objectCollision = GetComponent<ObjectCollision>();

        //当たった時の処理
        objectCollision.OnCollision
            .TakeUntilDestroy(this)
            .Subscribe(collision => Debug.Log("あたった！"));
    }

    /// <summary>
    /// ダメージ処理
    /// </summary>
    void AplayDamage()
    {
        Debug.Log("oh");
        ScoreManager.Instance.AddScore(1);
    }
}