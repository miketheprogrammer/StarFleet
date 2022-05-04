using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGhostCollisionMesh : MonoBehaviour
{
    [SerializeField]
    GameObject GhostCollisionMesh;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GhostCollisionMesh != null)
        {
            transform.position = GhostCollisionMesh.transform.position;
            transform.localScale = GhostCollisionMesh.transform.localScale;
            transform.rotation = GhostCollisionMesh.transform.rotation;
        }
    }
}
