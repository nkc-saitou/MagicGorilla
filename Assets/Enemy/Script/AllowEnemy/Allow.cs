using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Allow : MonoBehaviour {

    private float speed = 30f;

	void Start () {
		
	}

	void Update () {
        transform.position += speed * transform.forward * Time.deltaTime;
	}
}
