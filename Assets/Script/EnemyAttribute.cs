using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttribute : MonoBehaviour {


    AttackState attackState;

    public AttackState EnemyAttackType
    {
        get { return attackState; }
    }

    public HandType handType;

    public Attribute attributeType;

    private void Start()
    {
        attackState.attribute = attributeType;
        attackState.handType = handType;
    }
}
