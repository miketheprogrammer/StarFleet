using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class StartAsClient : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("NetworkManager").GetComponent<NetworkManager>().StartClient();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
