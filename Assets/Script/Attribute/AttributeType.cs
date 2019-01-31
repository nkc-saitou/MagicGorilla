using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct AttackState
{
    public Attribute attribute;
    public HandType handType;
}

public enum Attribute
{
    up = 0,
    down,
    left,
    right,
    none
}

public enum HandType
{
    rock = 0,
    paper,
    none
}

public class AttributeType : MonoBehaviour
{


}
