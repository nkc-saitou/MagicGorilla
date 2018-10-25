using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FVRlib;
/// <summary>
/// This example shows how to use the First VR's rotation and how to set the center or no rotation point
/// </summary>
public class SampleViewerManager : MonoBehaviour {

	// FVR 
	public FVRConnection fvr;

	// vars
	public GameObject cube;
	bool touched = false;

	void Start () {

	}


	void Update () {
		/// Giving the device's rotation to te cube.
		/// You can also get it from the reference object with fvr.referenceObj.transform.rotation
		/// Also if you prefer using your own centering algorithm you can get the FVR's raw rotation with fvr.rawRotation
		cube.transform.rotation = fvr.centeredRotation;

		/// Center the cube to currents device position with 1 finger screen tap.
		/// Calling the Recenter function will set the current Yaw  to no Yaw and the current Roll to no Roll.
		/// The Pitch stays unchanged so up and down will always be up and down.
		if (Input.touchCount == 1&&!touched) {
			fvr.Recenter ();
			touched = true;
		}
		if (Input.touchCount == 0&&touched) {			
			touched = false;
		}
	}
}
