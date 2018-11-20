using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllowRay : MonoBehaviour {
    public bool InSightTarget(Transform eye,Transform target)
    {
        bool flg = false;
        Vector3 vector3 = (eye.transform.position - target.position); 
        Ray ray = new Ray(eye.position, vector3);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "Player")//段差のタグ
            {
                flg = true;
            }
        }
        return flg;
    }
}
