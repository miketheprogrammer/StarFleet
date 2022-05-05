using Unity.Netcode;
using UnityEngine;

namespace networkSpawn
{
    public class networkSpawn : NetworkBehaviour
    {
       // Inside this class we now define a NetworkVariable to represent this player's networked position.
       public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        void Update()
        {
            transform.position = Position.Value;
        }
    }
}