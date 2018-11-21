using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowEnemyAttack : MonoBehaviour {

    public Transform offset;
    private const float ShiftRange = 0.8f;

    void Start()
    {

    }
    public void Attack(GameObject allow,Vector3 playerPos)
    {
        Vector3 shiftpos = Shift;
        Vector3 rot = (playerPos+shiftpos) - offset.position;
        Instantiate(allow, offset.position, Quaternion.LookRotation(rot));
    }



    Vector3 Shift//目標地点をずらす
    {
        get
        {
            Vector3 vector3;
            vector3.x = Random.Range(ShiftRange, -ShiftRange);
            vector3.y = Random.Range(ShiftRange, -ShiftRange);
            vector3.z = Random.Range(ShiftRange, -ShiftRange);
            return vector3;
        }
    }
}
