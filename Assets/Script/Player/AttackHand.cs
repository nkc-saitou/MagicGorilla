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
            .Where(collider => collider.GetComponent<ObjectType>() != null)
            .Subscribe(collider => 
            {
                BreakObj(collider.GetComponent<ObjectType>()._ObjectType,collider.gameObject);
            });

	}

    void BreakObj(E_ObjectType type,GameObject colObj)
    {
        BaseEnemy baseEnemy = colObj.GetComponent<BaseEnemy>();

        EnemyAttribute state = colObj.GetComponent<EnemyAttribute>();
        AttributeEnemyDamage attributeDamage = colObj.GetComponent<AttributeEnemyDamage>();

        if (state.EnemyAttackType.handType == handType && state.EnemyAttackType.attribute == attribute)
        {
            // ベースエネミーが基底のクラスだったら(WalkEnemy,BowEnemy)
            if (baseEnemy != null)
            {
                baseEnemy.EnemyHP--;
            }
            //BaseEnemyが実装されていないEnemyだったら(Attribute系のEnemy)
            else if (type == E_ObjectType.enemy)
            {
                if (attributeDamage != null)
                {
                    attributeDamage.Dead();
                }
            }
        }
        // Enemyが打ってくる障害物だったら(矢、岩など)
        else if (type == E_ObjectType.enemyObject)
        {
            Destroy(colObj);
        }

        Destroy(gameObject);
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.position += (transform.forward).normalized * speed * Time.deltaTime;
		
	}
}
