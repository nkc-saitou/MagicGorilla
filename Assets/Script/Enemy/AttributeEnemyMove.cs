using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeEnemyMove : MonoBehaviour {

    float speed = 1.0f;

	// Use this for initialization
	void Start ()
    {
        Destroy(gameObject, 30.0f);
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position += (-transform.forward).normalized * Time.deltaTime * speed;
	}
}
