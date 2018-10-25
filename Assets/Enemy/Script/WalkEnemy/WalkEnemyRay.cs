using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkEnemyRay : MonoBehaviour {
    private const float stepDis = 1.5f;     //段差検知距離
    private const float groundDis = 0.5f; //接地検知

    /// <summary>
    /// 目の前に段差があるか検知
    /// </summary>
    /// <param name="step">段差があるか</param>
    /// <returns></returns>
    public void StepDetection(out bool step)
    {
        bool flg1=false;
        Ray ray=new Ray(transform.position,transform.TransformDirection(Vector3.forward));
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, stepDis))
        {
            if (hit.collider.tag == "Untagged")//段差のタグ
            {
                flg1 = true;
            }
        }
        step = flg1;
        return;
    }

    /// <summary>
    /// 接地判定
    /// </summary>
    /// <returns></returns>
    public bool OnGround
    {
        get
        {
            bool flg = false;
            Ray ray = new Ray(transform.position, transform.TransformDirection(Vector3.down));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, groundDis))
            {
                if (hit.collider != null)
                {
                    flg = true;
                }
            }
            return flg;
        }
    }
}
