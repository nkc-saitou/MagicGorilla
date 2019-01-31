using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeEnemyMove : MonoBehaviour {

    float speed = 1.0f;

    public bool isMoveStop { get; set; }

	// Use this for initialization
	void Start ()
    {
        Destroy(gameObject, 30.0f);

        isMoveStop = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(isMoveStop == false) transform.position += (-transform.forward).normalized * Time.deltaTime * speed;
	}
}
