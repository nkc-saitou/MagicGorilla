using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddDamage : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if (enemy == null) return;

        enemy.EnemyHP--;
    }
}
