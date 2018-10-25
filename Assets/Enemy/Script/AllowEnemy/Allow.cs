using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Allow : MonoBehaviour {

    private float speed = 30f;

	void Start () {
        //自壊
        Destroy(gameObject, 7f);
    }

	void Update () {
        transform.position += speed * transform.forward * Time.deltaTime;
	}
}
