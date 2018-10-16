using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowEnemy : BaseEnemy {

    public GameObject allow;
    private int hp = 1;                         //hp
    private const float wait = 1f;              //発射間隔
    protected AllowEnemyAttack allowAttack;

    protected override void OnStart()
    {
        //体力設定
        EnemyHp = hp;
        StartCoroutine(AttackTime());
        allowAttack = GetComponent<AllowEnemyAttack>();
	}
	

	void Update () {
		
	}

    protected void Move()
    {

    }

    protected void Attack()
    {
        allowAttack.Attack(allow, PlayerPos.position);
    }

    protected IEnumerator AttackTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(wait);
            Attack();
        }
    }
}
