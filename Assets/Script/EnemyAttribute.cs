using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttribute : MonoBehaviour {


    AttackState attackState;

    public AttackState EnemyAttackType
    {
        get { return attackState; }
        set { EnemyAttackType = value; }
    }

    public HandType handType;

    public Attribute attributeType;

    private void Start()
    {
        attackState.attribute = attributeType;
        attackState.handType = handType;
    }

    public void SetAttributeType(Attribute attribute)
    {
        attackState.attribute = attribute;
    }

    public void SetHanType(HandType handType)
    {
        attackState.handType = handType;
    }
}
