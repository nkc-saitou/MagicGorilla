using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoneEnemyDamage : MonoBehaviour {

    public void AddDamage()
    {
        BaseEnemy enemy = GetComponent<BaseEnemy>();
        if (enemy == null) return;

        enemy.EnemyHP--;

    }
}
