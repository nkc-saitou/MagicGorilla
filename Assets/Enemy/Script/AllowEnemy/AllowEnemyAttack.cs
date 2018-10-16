using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowEnemyAttack : MonoBehaviour {

    public void Attack(GameObject allow,Vector3 playerPos)
    {
        Vector3 rot = playerPos - transform.position;
        Instantiate(allow, transform.position, Quaternion.LookRotation(rot));
    }
}
