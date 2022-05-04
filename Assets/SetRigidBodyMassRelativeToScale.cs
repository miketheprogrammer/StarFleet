using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SetRigidBodyMassRelativeToScale : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.mass = rb.mass / transform.localScale.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
