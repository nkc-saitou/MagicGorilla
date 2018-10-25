using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FVRlib;

/// <summary>
/// This example shows how to get and display the First VR's muscle sensor outputs.
/// </summary>

public class MSViewer : MonoBehaviour {

	// FVR
	FVRConnection fvr;

	// Texts
	Text[] msVals = new Text[14];

	void Start () {
		/// When using the FVRContainer prefab, the FVRConnection won't be destroyed on load allowing you to keep your calibrated gestures and centering data.
		/// Since the FVRConnection comes from a previous scene, you can't drag and drop it into a public variable when you are making the scene. 
		/// So when the new scene gets loaded you will need to find the FVRConnection instance in order to use it. 
		fvr = GameObject.Find("FVR").GetComponent<FVRConnection> ();

		// Getting the visual components
		msVals = transform.Find ("MsValues").GetComponentsInChildren<Text> ();
	}

	void Update () {
		// Displaying current muscle sensor values
		for (int i = 0; i < 14; i++) {
			msVals [i].text = "Ch. " + i.ToString () + " : " + fvr.muscleSensors [i].ToString ();
		}
	}
}
