using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamegeEne : MonoBehaviour {
    void OnTriggerEnter(Collider col)
    {
        if (col.GetComponent<BaseEnemy>())
        {
            col.GetComponent<BaseEnemy>().EnemyHP--;
        }
    }
}
