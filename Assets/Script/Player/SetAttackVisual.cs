using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

[RequireComponent(typeof(AttackHandShot))]
public class SetAttackVisual : MonoBehaviour {

    [Tooltip("0:グー、1:パー")]
    public Mesh[] handPre;

    public GameObject[] attribute;

    public MeshFilter displayHand;

    AttackHandShot attackHandShot;

    CreateAttributeType type;

    GestureInputState gestureState;

	// Use this for initialization
	void Start ()
    {

        attackHandShot = GetComponent<AttackHandShot>();

        int leftIndex = ((int)CreateAttributeType.left) - 1;
        int rightIndex = ((int)CreateAttributeType.right) - 1;

        this.UpdateAsObservable()
            .Subscribe(_ =>
            {
                type = attackHandShot.CreateType;

                gestureState = PlayerInput.Instance._GestureInputState;

            });

        this.ObserveEveryValueChanged(_ => type)
            .TakeUntilDestroy(this)
            .Subscribe(_ =>
            {
                switch(type)
                {
                    case CreateAttributeType.none:
                        attribute[leftIndex].SetActive(false);
                        attribute[rightIndex].SetActive(false);
                        break;

                    case CreateAttributeType.left:
                        attribute[leftIndex].SetActive(true);
                        attribute[rightIndex].SetActive(false);
                        break;

                    case CreateAttributeType.right:
                        attribute[leftIndex].SetActive(false);
                        attribute[rightIndex].SetActive(true);
                        break;
                }
            });

        this.ObserveEveryValueChanged(_ => gestureState)
            .Subscribe(_ => 
            {
                if (gestureState == GestureInputState.gestureRock)
                    displayHand.mesh = handPre[(int)GestureInputState.gestureRock];
                else
                    displayHand.mesh = handPre[(int)GestureInputState.gesturePaper];
            });

	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
