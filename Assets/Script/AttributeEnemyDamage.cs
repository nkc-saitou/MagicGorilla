using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeEnemyDamage : MonoBehaviour {

    Animator anim;

    AttributeEnemyMove move;

    public GameObject deadObj;

    private void Start()
    {

        move = GetComponent<AttributeEnemyMove>();
    }

    public void Dead()
    {
        if(move != null) move.isMoveStop = true;
        Instantiate(deadObj, transform.position, deadObj.transform.rotation);
        Destroy(gameObject);
    }

}
