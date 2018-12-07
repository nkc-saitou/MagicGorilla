using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;

public class RockShoot : MonoBehaviour {
    Rigidbody rb;

    public Vector3 localGravity = new Vector3(0, 5f, 0);

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        this.FixedUpdateAsObservable().
            TakeUntilDestroy(this).
            Subscribe(_ => setLocalGravity());

        Destroy(gameObject, 3);

    }

    void setLocalGravity()
    {
        rb.AddForce(-localGravity, ForceMode.Acceleration);
    }
}
