using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRandomRotation : MonoBehaviour
{
    [SerializeField]
    Vector2 minMax = new Vector2(0f, 180f);

    enum Axis
    {
        X,
        Y,
        Z,
    }

    [SerializeField]
    Axis rotationAxis = Axis.Z;
    // Start is called before the first frame update
    void Start()
    {
        float rotation = Random.RandomRange(minMax.x, minMax.y);
        if (rotationAxis == Axis.Z)
        {
            transform.rotation = Quaternion.Euler(0, 0, rotation);
        }
        if (rotationAxis == Axis.X)
        {
            transform.rotation = Quaternion.Euler(rotation, 0, 0);
        }
        if (rotationAxis == Axis.Y)
        {
            transform.rotation = Quaternion.Euler(0, rotation, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
