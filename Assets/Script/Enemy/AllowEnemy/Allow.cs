using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;

public class Allow : MonoBehaviour {

    private float speed = 30f;

	void Start () {
        //自壊
        Destroy(gameObject, 7f);

        this.FixedUpdateAsObservable().
            TakeUntilDestroy(this).
            Subscribe(_ => transform.position += speed * transform.forward * Time.deltaTime);
    }
}
