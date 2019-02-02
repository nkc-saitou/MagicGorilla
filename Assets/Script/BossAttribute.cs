using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;

public class BossAttribute : MonoBehaviour {

    public GameObject[] collider;

    List<ObjectCollision> objColLis = new List<ObjectCollision>();

    List<AttributeType> typeLis = new List<AttributeType>();


    HandType handType;

    Attribute type;

    public BaseEnemy baseEnemy;

    public SpriteRenderer handTypeImage;

    public Sprite[] sp;

    float t = 0;

    float interbarTime = 10.0f;

    int interbarCount;

    bool isChangeAttribute = false;

    bool isChangeHand = false;

	// Use this for initialization
	void Start ()
    {
        handType = HandType.paper;

        for (int i = 0; i < collider.Length; i++)
        {
            objColLis.Add(collider[i].GetComponent<ObjectCollision>());
        }

        Debug.Log(objColLis.Count);

        for (int i = 0; i < objColLis.Count; i++)
        {
            Debug.Log(objColLis[i]);

            objColLis[i].OnCollision
                .Where(collisin => collisin.GetComponent<AttackHand>() != null)
                .Subscribe(collision =>
                {
                    AttackHand hand = collision.GetComponent<AttackHand>();
                    if (hand._AttackState.attribute == objColLis[i].GetComponent<EnemyAttribute>().attributeType && hand._AttackState.handType == handType)
                    {
                        baseEnemy.EnemyHP--;
                    }
                });
        }

    }
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log(baseEnemy.EnemyHP);
        AttributeChangeTime(interbarTime);
        ChangeAttribute();

        if(interbarCount >= 3)
        {
            interbarCount = 0;
            interbarTime -= 2.0f;
        }

        
        
    }

    void HandChangeTime(float handInterbar)
    {
        t += Time.deltaTime;

        if (t > handInterbar)
        {
            t = 0;
            isChangeHand = true;
        }
    }

    void HandChange()
    {
        if (isChangeHand == false) return;

        isChangeHand = false;

        if (handType == HandType.rock) handType = HandType.paper;
        else handType = HandType.rock;
    }

    void AttributeChangeTime(float interbar)
    {
        t += Time.deltaTime;

        if (t > interbar)
        {
            isChangeAttribute = true;
            t = 0;
            interbarCount++;
        }
    }

    void ChangeAttribute()
    {
        if(isChangeAttribute == true)
        {
            isChangeAttribute = false;

            int range = Random.Range(0, collider.Length);

            for(int i = 0; i < collider.Length; i++)
            {
                if (i == range)
                {
                    collider[range].SetActive(true);
                    return;
                }
                else collider[i].SetActive(false);
            }
        }
    }
}
