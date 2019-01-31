using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeEnemyMove : MonoBehaviour {

    float speed = 1.0f;

    public bool isMoveStop { get; set; }

    float time = 0;

    SpawnManeger manager;

	// Use this for initialization
	void Start ()
    {
        Destroy(gameObject, 30.0f);

        manager = FindObjectOfType<SpawnManeger>();

        isMoveStop = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(isMoveStop == false) transform.position += (-transform.forward).normalized * Time.deltaTime * speed;

        time += Time.deltaTime;

        if (time >= 29.0f)
        {
            time = 0;
            manager.WallAttack();
        }

    }
}
