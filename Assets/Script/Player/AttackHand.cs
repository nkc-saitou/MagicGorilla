using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHand : MonoBehaviour {

    public AttackState _AttackState
    {
        get { return state; }
    }

    AttackState state;

    public Attribute attribute;

    public HandType handType;

    float speed = 5.0f;

    Transform movePos;

	// Use this for initialization
	void Start ()
    {
        state.attribute = attribute;
        state.handType = handType;

        movePos = GameObject.FindGameObjectWithTag("LeftHandAnchor").transform;
        transform.LookAt(movePos);

        Destroy(gameObject, 5.0f);

	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position += (transform.forward).normalized * speed * Time.deltaTime;
		
	}
}
