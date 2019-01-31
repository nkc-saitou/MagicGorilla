using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class AttackHand : MonoBehaviour {

    public AttackState _AttackState
    {
        get { return state; }
    }

    ObjectCollision objCollision;

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

        objCollision = GetComponent<ObjectCollision>();

        movePos = GameObject.FindGameObjectWithTag("LeftHandAnchor").transform;
        transform.LookAt(movePos);

        Destroy(gameObject, 5.0f);


        objCollision.OnCollision
            .Where(collider => collider.GetComponent<ObjectCollision>() != null)
            .Subscribe(collider => 
            {

            });

	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position += (transform.forward).normalized * speed * Time.deltaTime;
		
	}
}
