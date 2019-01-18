using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class RagDollDelete : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Observable.Timer(System.TimeSpan.FromSeconds(0.05f)).
        Subscribe(_ => GetComponent<Animator>().enabled = false);
        
        Destroy(gameObject, 3);
    }	
	
}
