using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeEnemyDamage : MonoBehaviour {

    Animator anim;

    AttributeEnemyMove move;

    private void Start()
    {

        move = GetComponent<AttributeEnemyMove>();
    }

    public void Dead()
    {
        move.isMoveStop = true;
        Destroy(gameObject, 1.0f);
    }

}
