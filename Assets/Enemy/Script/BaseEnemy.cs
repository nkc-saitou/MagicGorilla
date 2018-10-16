using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemy : MonoBehaviour {
    //プレイヤーの座標
    public Transform PlayerPos;//{ get; set; }
    //ナビメッシュ
    protected NavMeshAgent agent;
    //移動用パス
    protected NavMeshPath path;
    //体力
    protected float EnemyHp { get; set; }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        OnStart();
    }

    /// <summary>
    /// Startと同義
    /// </summary>
    protected virtual void OnStart(){}

}
