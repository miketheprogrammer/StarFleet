using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SetRandomScale : MonoBehaviour
{
    [SerializeField]
    Vector2 minMax = new Vector2();
    // Start is called before the first frame update
    void Start()
    {
        if (!Application.isPlaying)
        {
            float scale = Random.RandomRange(minMax.x, minMax.y);
            transform.localScale = new Vector3(scale, scale, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
