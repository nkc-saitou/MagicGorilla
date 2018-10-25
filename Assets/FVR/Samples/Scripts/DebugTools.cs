using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using FVRlib;
/// <summary>
/// This example shows how to get and display the First VR's basic outputs.
/// </summary>
public class DebugTools : MonoBehaviour {
	
	// FVR
	FVRConnection fvr;
	FVRGestureManager gm;

	// Texts
	Text yaw, pitch, roll;
	Text w, accel;
	Text verOr;
	Text horOr;
	Text palmOr;

	// Images
	Image[] orientationImgs = new Image[25];
	Image[] swipeImgs = new Image[5];
	Image highAccelImg, shakeImg;

	//Control variables
	public float highAccelTriggerVal = 1.2f;

	void Awake(){
		/// When using the FVRContainer prefab, the FVRConnection won't be destroyed on load allowing you to keep your calibrated gestures and centering data.
		/// Since the FVRConnection comes from a previous scene, you can't drag and drop it into a public variable when you are making the scene. 
		/// So when the new scene gets loaded you will need to find the FVRConnection instance in order to use it. 
		fvr = GameObject.Find("FVR").GetComponent<FVRConnection> ();
		gm = fvr.gameObject.GetComponent<FVRGestureManager>();
	}

	//Subscribing the event handlers
	/// <summary>
	/// This is one way to listen to gesture events.
	/// For people not accustomed to event systems, delegates, handlers and all that fun stuff. There is an other way to know if an event has been triggered.
	/// There is an example of this in the Update() function in this script.
	/// For more information look up FVRGesture.triggered in the documentation.
	/// </summary>
	void OnEnable(){
		gm.OnSwipe += HandleSwipe;
		gm.OnSwipeUp += HandleSwipeU;
		gm.OnSwipeDown += HandleSwipeD;
		gm.OnSwipeLeft += HandleSwipeL;
		gm.OnSwipeRight += HandleSwipeR;
		gm.OnShake += HandleShake;
	}

	void Start () {	
		// Obtaining visual components
		orientationImgs = transform.Find ("OrientationImgs").GetComponentsInChildren<Image> ();

		for (int i = 0; i < 5; i++) {
			swipeImgs [i] = transform.Find ("SwipeImgs/" + i).GetComponent<Image> ();
		}

		highAccelImg = transform.Find ("HighAccel").GetComponent<Image> ();
		shakeImg = transform.Find ("Shake").GetComponent<Image> ();

		yaw = transform.Find ("Yaw").GetComponent<Text> ();
		pitch = transform.Find ("Pitch").GetComponent<Text> ();
		roll = transform.Find ("Roll").GetComponent<Text> ();
		w = transform.Find ("W").GetComponent<Text> ();
		accel = transform.Find ("Accel").GetComponent<Text> ();
		verOr = transform.Find ("vOr").GetComponent<Text> ();
		horOr = transform.Find ("hOr").GetComponent<Text> ();
		palmOr = transform.Find ("pOr").GetComponent<Text> ();			

	}


	void Update () {
		UpdateTexts ();
		AccelColors ();
		UpdateOrientation ();
		/// This is other way of checking for gesture events
		if (gm.shake.triggered) {
			// Do something here when the shake event is triggered
			// Every FVRGesture has a "triggered" boolean which is only true during the frame the event was triggered
			// In addition there is a "held" boolean which will be true during the duration of an event.
		}
	}
	/// <summary>
	/// Updates the text components with the values obtained from the FVRConnection
	/// </summary>
	void UpdateTexts(){
		yaw.text = "Yaw : "+fvr.Yaw.ToString ("0.##"); 
		pitch.text = "Pitch : "+fvr.Pitch.ToString ("0.##");
		roll.text = "Roll : "+fvr.Roll.ToString ("0.##");
		accel.text = "Magnitude : "+Mathf.Sqrt (Mathf.Pow (fvr.rawAccel [0], 2) + Mathf.Pow (fvr.rawAccel [1], 2) + Mathf.Pow (fvr.rawAccel [2], 2)).ToString ("0.##");
		w.text = "Magnitude : "+fvr.angularVelocity.magnitude.ToString("0.##");
		horOr.text =  "Horizontal:\t"+fvr.horizontalOrientation.ToString ();
		verOr.text =  "Vertical:\t\t"+fvr.verticalOrientation.ToString ();
		palmOr.text = "Palm:\t\t\t" + fvr.palmOrientation.ToString();
	}

	/// <summary>
	/// Updates the colors of the orientation map.
	/// The FVRDevice prefab contains 26 orientation panels surrounding the device.
	/// When properly centered (sample of how to center in SampleViewManager.cs) the FVRConnection.orientPanel will always be the panel the user's arm is pointing at.
	/// This example uses the GameObject.name to identify the orientation, but the FVROrientationPanel stores orientation data in several properties.
	/// Check the documentation on FVROrientationPanel for more information. 
	/// </summary>
	void UpdateOrientation(){
		for (int i = 0; i < 26; i++) {
			if (orientationImgs[i].name == fvr.orientPanel.name) {
				orientationImgs [i].color = Color.green;
			} else {
				orientationImgs [i].color = Color.white;
			}
		}

	}

	/// <summary>
	/// The FVRConnection.accel vector can be used as a trigger for events.
	/// This value is related to the accelerometer and is not affected by gravity, but it does not represent the real acceleration value in g's or m/s^2.
	/// It is merely a reference vector with a magnitude of 0.5f when there is no acceleration and when accelerated the magnitude also grows.
	/// This magnitude is useful as a trigger for events but should not be used for any mathematical calculations.
	/// </summary>
	void AccelColors(){
		if (fvr.accel.magnitude > highAccelTriggerVal) {
			highAccelImg.color = Color.red;
		} else {
			highAccelImg.color = Color.white;
		}
	}

	//Event Handlers
	void HandleSwipeU(){
		swipeImgs[1].color = Color.green;
		StartCoroutine (reset ("U"));

	}
	void HandleSwipeD(){
		swipeImgs[3].color = Color.green;
		StartCoroutine (reset ("D"));

	}
	void HandleSwipeL(){
		swipeImgs[4].color = Color.green;
		StartCoroutine (reset ("L"));

	}
	void HandleSwipeR(){
		swipeImgs[2].color = Color.green;
		StartCoroutine (reset ("R"));

	}
	void HandleSwipe(){
		swipeImgs[0].color = Color.green;
		StartCoroutine (reset ("A"));

	}

	void HandleShake(){
		shakeImg.color = Color.green;
		StartCoroutine (reset ("S"));
	}


	IEnumerator reset(string dir){
		yield return new WaitForSeconds (1f);
		switch (dir) {
		case "U":
			swipeImgs[1].color = Color.white;
			break;
		case "D":
			swipeImgs[3].color =Color.white;
			break;
		case "L":
			swipeImgs[4].color = Color.white;
			break;
		case "R":
			swipeImgs[2].color =Color.white;
			break;
		case "A":
			swipeImgs[0].color = Color.white;
			break;
		case "S":
			shakeImg.color = Color.white;
			break;

		}
	}

	// Remember to unsubscribe your handlers when done!
	void OnDisable(){
		gm.OnSwipe -= HandleSwipe;
		gm.OnSwipeUp -= HandleSwipeU;
		gm.OnSwipeDown -= HandleSwipeD;
		gm.OnSwipeLeft -= HandleSwipeL;
		gm.OnSwipeRight -= HandleSwipeR;
		gm.OnShake -= HandleShake;
	}
}
