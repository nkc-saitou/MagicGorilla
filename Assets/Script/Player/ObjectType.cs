using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_ObjectType
{
    enemy = 0,
    enemyObject,
    player,
    playerHand,
    wall,
}

public class ObjectType : MonoBehaviour {

    public E_ObjectType objectType;

    public E_ObjectType _ObjectType { get { return objectType; } }
}
