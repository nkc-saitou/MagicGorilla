using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRandom : MonoBehaviour {

    float t;

    int interbarCount;

    float interbarTime = 10.0f;

    bool isChangeAttribute = false;

    EnemyAttribute attribute;

    HandType handType;
    Attribute attributeType;

    public GameObject Effect;

    // Use this for initialization
    void Start () {
        Effect.SetActive(false);
        attribute = GetComponent<EnemyAttribute>();
    }
	
	// Update is called once per frame
	void Update () {

        if (interbarCount >= 3)
        {
            interbarCount = 0;
            interbarTime -= 1.0f;
        }

        AttributeChangeTime();
        ChangeAttribute();
    }

    void AttributeChangeTime()
    {
        t += Time.deltaTime;

        if (t > interbarTime)
        {
            isChangeAttribute = true;
            Effect.SetActive(false);
            t = 0;
            interbarCount++;
        }
    }

    void ChangeAttribute()
    {
        if(isChangeAttribute == true)
        {
            isChangeAttribute = false;

            Effect.SetActive(true);
            int handRange = Random.Range(0, 2);
            int attributeRand = Random.Range(0, 3);

            Debug.Log(handRange + "    " + attributeRand);

            if (handRange == 0) attribute.SetHanType(HandType.rock);
            else attribute.SetHanType(HandType.paper);

            if (attributeRand == 0) attribute.SetAttributeType(Attribute.up);
            else if (attributeRand == 1) attribute.SetAttributeType(Attribute.down);
            else attribute.SetAttributeType(Attribute.none);
        }
    }
}
