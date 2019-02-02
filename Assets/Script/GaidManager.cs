using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class GaidManager : MonoBehaviour {

    public bool IsObjectActive { get; set; }

    bool isSerif = false; // セリフが流れているかどうか

    bool isTemp = false;//毎フレーム回さないため

    BaseGaid[] baseGaid = new BaseGaid[3];

    BaseGaid choiceGaid;

    GameObject choiceGaidObj;

    int gaidNum = 3;

    private void Start()
    {
        for(int i = 0; i < gaidNum; i++)
        {
            baseGaid[i] = transform.GetChild(i).GetComponent<BaseGaid>();
        }

        GaidChoice();

        IsObjectActive = true;
    }

    public void ObjectNonActive()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetChild(0).GetComponent<Animator>().enabled = false;
        foreach (Transform mChild in transform.GetChild(i))
            {
                mChild.gameObject.SetActive(false);
            }
        }
    }

    public void ObjectActive()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetChild(0).GetComponent<Animator>().enabled = true;
            foreach (Transform mChild in transform.GetChild(i))
            {
                StartCoroutine(ImageActive(mChild.gameObject));
            }
        }
    }

    private void Update()
    {
        isSerif = SerifManager.Instance.IsSerinSend;

        if (isSerif == true)
        {
            isTemp = true;


            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetChild(0).GetComponent<Animator>().enabled = false;
                foreach (Transform mChild in transform.GetChild(i))
                {
                    mChild.gameObject.SetActive(false);
                }
            }
        }
        else if (isTemp)
        {
            isTemp = false;

            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetChild(0).GetComponent<Animator>().enabled = true;
                foreach (Transform mChild in transform.GetChild(i))
                {
                    StartCoroutine(ImageActive(mChild.gameObject));
                }
            }
        }

        if (PlayerInput.Instance.HitGameObject == null) return;

        choiceGaidObj = PlayerInput.Instance.HitGameObject.transform.parent.gameObject;

        if (Input.GetMouseButtonDown(0) || OVRInput.GetDown(OVRInput.Touch.One))
        {
            choiceGaid = ChoiceGaid(choiceGaidObj);
            choiceGaid.GaidAction();
            PlayerInput.Instance.HitGameObject = null;
        }
    }

    IEnumerator ImageActive(GameObject child,float time = 1.3f)
    {
        yield return new WaitForSeconds(1.0f);
        child.gameObject.SetActive(true);
    }


    BaseGaid ChoiceGaid(GameObject gaid)
    {
        for(int i = 0; i < gaidNum; i++)
        {
            if (baseGaid[i].gameObject == gaid) return baseGaid[i];
        }

        return null;
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
