using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FVRlib;

public class ArmMove : MonoBehaviour {

    FVRConnection fvr;

    bool touched = false;

    int heldCount;
    int nonHeldCount;

	// Use this for initialization
	void Start () {
        fvr = FindObjectOfType<FVRConnection>();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR

#else
        Quaternion fvrRotation = fvr.centeredRotation;

        transform.rotation = fvrRotation;

        if (Input.touchCount == 1 && !touched)
        {
            fvr.Recenter();
            touched = true;
        }
        if (Input.touchCount == 0 && touched)
        {
            touched = false;
        }
#endif
    }
}
