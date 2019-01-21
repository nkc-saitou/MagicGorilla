using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class RagDollDelete : MonoBehaviour {

    [SerializeField]
    Rigidbody body;

    float power;
	void Start ()
    {
        power = Random.Range(2500f, 7000f);
        Observable.Timer(System.TimeSpan.FromSeconds(0.05f)).
        Subscribe(_ => GetComponent<Animator>().enabled = false);
        body.AddForce(-transform.forward*power);
        Destroy(gameObject, 3);
    }	
}
