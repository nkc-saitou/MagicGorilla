using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Golem : BaseEnemy
{
    [SerializeField]
    private float hp = 50;


	void Start () {
        EnemyHP = hp;
	}
	

	void Update () {
		
	}

    protected override void Dead()
    {
        Debug.Log("dead");
        Destroy(gameObject, 0.001f);
    }    
}
