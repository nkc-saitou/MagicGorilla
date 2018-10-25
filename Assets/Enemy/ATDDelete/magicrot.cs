using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magicrot : MonoBehaviour {
    public float speed = 1;
    public Vector3 rot ;
	// Use this for initialization
	void Start () {
		
	}
	
	//回るだけ
	void Update () {
        transform.Rotate(rot*speed * Time.deltaTime, Space.World);
    }

}
