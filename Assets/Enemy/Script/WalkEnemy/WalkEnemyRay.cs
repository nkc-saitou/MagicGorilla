using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkEnemyRay : MonoBehaviour {
    private const float stepDis = 1.5f;     //段差検知距離
    private const float groundDis = 0.5f; //接地検知

    /// <summary>
    //目の前に物体があるか検知
    /// </summary>
    public bool StepDetection(out bool step)
    {
        bool flg=false;
        Ray ray=new Ray(transform.position,transform.TransformDirection(Vector3.forward));
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, stepDis))
        {
            if (hit.collider.tag == "Untagged")//段差のタグ
            {
                flg = true;
            }
        }
        return step=flg;
    }

    /// <summary>
    /// 接地判定
    /// </summary>
    /// <returns></returns>
    public bool OnGround(out bool ground)
    {
        bool flg = false;
        Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.down));
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, groundDis))
        {
            if (hit.collider!=null)
            {
                flg = true;
            }
        }
        return ground=flg;
    }
}
