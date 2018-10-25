using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using FVRlib;
/// <summary>
/// This example shows how to create, calibrate and test a custom gesture.
/// </summary>
public class CalibTestCtrl : MonoBehaviour {

	// FVR 
	FVRConnection fvr;
	FVRGesture gesture;

	//Control variables
	int samplesPerSecond = 0;
	int roundLength = 0;
	int tCalibRounds = 0;
	int ntCalibRounds = 0;

	// Texts
	public Text samplesPerSecondTxt;
	public Text roundLengthTxt;
	public Text tCalibRoundsTxt;
	public Text ntCalibRoundsTxt;

	// Images
	public Image targetImg;
	public Image nonTargetImg;
	public Image testImg;

	//Buttons
	public Button targetBtn;
	public Button nonTargetBtn;
	public Button resetBtn;
	public Button[] varBtns;

	void Start () {
		/// When using the FVRContainer prefab, the FVRConnection won't be destroyed on load allowing you to keep your calibrated gestures and centering data.
		/// Since the FVRConnection comes from a previous scene, you can't drag and drop it into a public variable when you are making the scene. 
		/// So when the new scene gets loaded you will need to find the FVRConnection instance in order to use it. 
		fvr = FindObjectOfType (typeof(FVRConnection)) as FVRConnection;

		// Create a new custom gesture
		gesture = fvr.gestureManager.RegisterCustomGesture ("gestureName");

		// Display the default settings
		samplesPerSecond = fvr.gestureManager.calibrationSamplesPerSecond;
		roundLength = (int)fvr.gestureManager.calibrationRoundLength;
		UpdateTexts ();

		// Button control
		targetBtn.interactable = false;
	}	

	void Update () {
		/// Custom gestures like all other FVR gestures, have a triggered and a held flag. which can be used for events.
		/// In this example, to test the gesture, we simply check if the gesture is being held and change the color of an image accordingly
		if (gesture.held) {
			testImg.color = Color.green;
		} else {
			testImg.color = Color.white;
		}
	}

	/// <summary>
	/// You can change the amount of samples that are to be taken every second of the calibration round, and fed into the svm.
	/// This value should always be higher than 1 and is limited by processing speeds.
	/// Recomended values are 10~20
	/// </summary>
	public void ChangeSPS(int dir){
		samplesPerSecond += 1 * dir;
		samplesPerSecond = samplesPerSecond < 1 ? 1 : samplesPerSecond;
		fvr.gestureManager.calibrationSamplesPerSecond = samplesPerSecond;
		UpdateTexts ();
	}
	/// <summary>
	/// You can change the length of the calibration round.
	/// This length should always be higher than 0 and making it too long might affect the results in a negative way.
	/// Recomended values are 1~3
	/// </summary>
	public void ChangeRL(int dir){		
		roundLength += 1 * dir;
		roundLength = roundLength < 1 ? 1 : roundLength;
		fvr.gestureManager.calibrationRoundLength = (float)roundLength;
		UpdateTexts ();
	}

	public void SetTargetPress(){
		StartCoroutine (Calibrate (true));
	}

	public void SetNonTargetPress(){
		StartCoroutine (Calibrate (false));
	}

	// Reset the calibration data and start all over again
	public void ResetCalibrationPress(){
		fvr.gestureManager.ResetPatternData (gesture);
		tCalibRounds = 0;
		ntCalibRounds = 0;
		UpdateTexts ();
		targetBtn.interactable = false;
		nonTargetBtn.GetComponentInChildren<Text> ().text = "Set\nDummy";
		foreach (Button b in varBtns) {
			b.interactable = true;
		}
	}

	// Updates the display texts
	void UpdateTexts(){
		tCalibRoundsTxt.text = tCalibRounds.ToString ();
		ntCalibRoundsTxt.text = ntCalibRounds.ToString ();
		samplesPerSecondTxt.text = samplesPerSecond.ToString ();
		roundLengthTxt.text = roundLength.ToString ();
	}

	/// <summary>
	/// Calibrate the gesture with target or non-target values.
	/// Calibration requires time, and it's best to let the user know what's going on, so this process is best done in a coroutine.
	/// </summary>
	IEnumerator Calibrate(bool target){
		if (target) {
			// Setting target values
			fvr.gestureManager.SetTargetData (gesture);
			tCalibRounds++;
		} else {
			// Setting non-target values
			fvr.gestureManager.SetNonTargetData (gesture);
			/// The first time we set a target or non-target value, the round length and samples per second are ignored and the SVM takes only one value with dummy data then
			/// the dummy data is replaced with real data. 
			/// After the first round the FVRGesture.calibrated flag is set to true and you are ready to start calibrating with real data
			if (gesture.calibrated) {
				ntCalibRounds++;
			}else{
				nonTargetBtn.GetComponentInChildren<Text> ().text = "Set\nNonTarget";
				foreach (Button b in varBtns) {
					b.interactable = false;
				}
			}
		}
		// We dont wan't multiple coroutines taking the same data so it's good to block the user from starting a new one before this round is done
		targetBtn.interactable = false;
		nonTargetBtn.interactable = false;
		resetBtn.interactable = false;
		float t = 0;
		while (gesture.registering) {
			/// While the target or non-target data is being set, the FVRGesture.registering flag will be set to true.
			/// A count down or a image fill loading bar is a good way to let the user know your app is doing something.
			/// Once the porcess is done, the FVRGesture.registering flag will be set to false, and we will exit this while loop.
			t += Time.deltaTime;
			if(target)
				targetImg.fillAmount = t / (float)roundLength;
			else
				nonTargetImg.fillAmount = t / (float)roundLength;
			yield return null;
		}
		UpdateTexts ();
		targetImg.fillAmount = 0;
		nonTargetImg.fillAmount =0;
		// After the process is done you can enable whatever buttons you need to proceed with the calibration or move on with your app.
		targetBtn.interactable = true;
		nonTargetBtn.interactable = true;
		resetBtn.interactable = true;
	}
}
