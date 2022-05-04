using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ManageVfxVelocity : MonoBehaviour
{
    float velocity = 0f;
    Vector3 positionLastFrame;
    Vector3 direction = Vector3.zero;
    [SerializeField]
    VisualEffect effect;
    // Start is called before the first frame update
    void Start()
    {
        positionLastFrame = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        direction = (transform.position - positionLastFrame).normalized;
        Debug.Log(transform.position);
        Debug.Log(direction);
        velocity = Vector3.Distance(transform.position, positionLastFrame) / Time.fixedDeltaTime;

        effect.SetVector3("Velocity", direction * -velocity);

        positionLastFrame = transform.position;
    }
}
