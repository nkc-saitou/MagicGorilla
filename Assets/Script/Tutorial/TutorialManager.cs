using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public enum TutorialState
{
    none = 0,
    t_1 ,
    t_2,
    t_3,
    t_4,
    end
}

public class TutorialManager : MonoBehaviour {

    float first_t = 0;

    float timeInterbar = 0;

    bool isAccel = false;

    bool isT_4 = false;
    bool isEnd = false;

    TutorialState state;

    public string[] t_1;
    public string[] t_2;
    public string[] t_3;
    public string[] t_4;

    public GameObject monster;

    GameObject obj_t_2;
    GameObject obj_t_3;

    // Use this for initialization
    void Start ()
    {
        PlayerInput.Instance.OnAccel
            .Where(_ => GameModeManager.Instance._GameState != GameState.game)
            .Subscribe(_ =>
            {
                Debug.Log(GameModeManager.Instance._GameState);
                StartCoroutine(Serif_t_3());
            });
	}
	
	// Update is called once per frame
	void Update ()
    {
        StartCoroutine(Serif_t_1());
        StartCoroutine(Serif_t_2());
        StartCoroutine(Serif_t_4());
        StartCoroutine(TutorialEnd());
    }

    //void Serif()
    //{
    //    if (IsFirst() != true) return;

    //    if (isFirst == false)
    //    {
    //        isFirst = true;
    //        state = TutorialState.t_1;
    //        SerifManager.Instance.SerifStart(t_1, "t_1");
    //    }
    //}

    //void Serif_t_2()
    //{
    //    Debug.Log(SerifManager.Instance.IsChat(t_1, "t_1"));

    //    if (SerifManager.Instance.IsChat(t_1, "t_1") == false && state == TutorialState.t_1)
    //    {
    //        state = TutorialState.t_2;
    //        GameObject obj = Instantiate(monster, monster.transform.position, monster.transform.rotation);

    //        isStart_t_2 = true;
    //    }
    //}

    IEnumerator Serif_t_1()
    {
        if(state == TutorialState.none)
        {

            yield return new WaitForSeconds(1.0f);

            SerifManager.Instance.SerifStart(t_1, "t_2");

            Debug.Log("t_1");

            state = TutorialState.t_1;
        }
    }

    IEnumerator Serif_t_2()
    {
        if (SerifManager.Instance.IndexNumber >= t_1.Length - 1 && SerifManager.Instance.IsChat(t_1, "t_1") == false && state == TutorialState.t_1)
        {
            state = TutorialState.t_2;
            yield return new WaitForSeconds(1.0f);
            obj_t_2 = Instantiate(monster, monster.transform.position, monster.transform.rotation);

            yield return new WaitForSeconds(1.0f);

            SerifManager.Instance.SerifStart(t_2, "t_2");
        }

    }

    IEnumerator Serif_t_3()
    {
        if (SerifManager.Instance.IsChat(t_2, "t_2") == false && state == TutorialState.t_2)
        {
            state = TutorialState.t_3;
            yield return new WaitForSeconds(1.0f);

            SerifManager.Instance.SerifStart(t_3, "t_3");

            yield return new WaitForSeconds(1.0f);
            if(obj_t_2 != null) Destroy(obj_t_2);
            obj_t_3 = Instantiate(monster, monster.transform.position, monster.transform.rotation);

            isT_4 = true;
        }
    }

    IEnumerator Serif_t_4()
    {
        if(isT_4 && obj_t_3 == null && SerifManager.Instance.IsChat(t_3, "t_3") == false && state == TutorialState.t_3)
        {
            state = TutorialState.t_4;

            yield return new WaitForSeconds(1.0f);

            SerifManager.Instance.SerifStart(t_4,"t_4");

            isEnd = true;
        }
    }

    IEnumerator TutorialEnd()
    {
        if (isEnd && SerifManager.Instance.IsChat(t_4, "t_4") == false && state == TutorialState.t_4)
        {
            state = TutorialState.end;

            yield return new WaitForSeconds(1.0f);

            EffectManager.Instance.FadeScene("Title");
        }
    }
}
