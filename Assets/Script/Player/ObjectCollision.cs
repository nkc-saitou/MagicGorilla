using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ObjectCollision : MonoBehaviour {

    //何かにあたった時にイベントを発行するためのインスタンス
    Subject<Collider> collSubject = new Subject<Collider>();

    //あたったものが何かを公開
    public IObservable<Collider> OnCollision
    {
        get { return collSubject; }
    }

    void OnTriggerEnter(Collider other)
    {
        //何かに当たった時にイベントを発行
        collSubject.OnNext(other);

        Debug.Log(other);
    }
}
