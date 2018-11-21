using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRay : MonoBehaviour {
    private const float stepDis = 1.6f;     //段差検知距離
    private const float groundDis = 0.5f;     //接地検知
    private const int layerMask = ~(1 << 9);//レイヤーマスク
    public Vector3 offset;

    /// <summary>
    /// 目の前に段差があるか検知
    /// </summary>
    /// <param name="step">段差があるか</param>
    /// <returns></returns>
    public bool StepDetection
    {
        get
        {
            bool flg1 = false;
            Ray ray = new Ray(transform.position + offset, transform.TransformDirection(Vector3.forward));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, stepDis,layerMask))
            {
                if (hit.collider.tag == "Untagged")//段差のタグ
                {
                    flg1 = true;
                }
            }
            return flg1;
        }
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
            Ray ray = new Ray(transform.position + offset, transform.TransformDirection(Vector3.down));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, groundDis, layerMask))
            {
                if (hit.collider != null)
                {
                    flg = true;
                }
            }
            return flg;
        }
    }

    /// <summary>
    /// 対象までに障害がないか
    /// </summary>
    /// <param name="eye"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool InSightTarget(Transform eye, Transform target)
    {
        bool flg = false;
        Vector3 vector3 = (target.position-eye.transform.position);
        Ray ray = new Ray(eye.position, vector3);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit,20f, layerMask))
        {
            if (hit.collider.tag == "Player")//段差のタグ
            {
                flg = true;
            }
        }
        return flg;
    }
}
