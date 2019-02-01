using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class GaidManager : MonoBehaviour {

    bool isSerif = false; // セリフが流れているかどうか

    bool isTemp = false;//毎フレーム回さないため

    BaseGaid[] baseGaid = new BaseGaid[3];

    int gaidNum = 3;

    private void Start()
    {
        for(int i = 0; i < gaidNum; i++)
        {
            baseGaid[i] = transform.GetChild(i).GetComponent<BaseGaid>();
        }

        GaidChoice();
    }

    private void Update()
    {
        isSerif = SerifManager.Instance.IsSerinSend;

        if(isSerif == true)
        {
            isTemp = true;
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        else if(isTemp)
        {
            isTemp = false;
            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    void GaidChoice()
    {
        this.ObserveEveryValueChanged(_ => PlayerInput.Instance.HitGameObject)
            .Subscribe(_ => 
            {
                for (int i = 0; i < gaidNum; i++)
                {
                    baseGaid[i].GaidAnim();
                }
            });
    }
}
