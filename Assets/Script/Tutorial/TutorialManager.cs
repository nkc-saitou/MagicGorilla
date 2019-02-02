using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    float t = 0;

    bool isDelay = false; // 初めの話出しを少しだけ遅らせる

    bool isFirst = false; // 一度だけ再生

    public string[] t_1;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Serif();

    }

    bool IsFirst()
    {
        if (isDelay == false)
        {
            t += Time.deltaTime;
            if (t >= 1.0f)
            {
                t = 0;
                isDelay = true;
                return true;
            }
        }
        return false;
    }

    void Serif()
    {
        if (IsFirst() != true) return;

        if (isFirst == false)
        {
            isFirst = true;
            SerifManager.Instance.SerifStart(t_1, "t_1");
        }
    }
}
