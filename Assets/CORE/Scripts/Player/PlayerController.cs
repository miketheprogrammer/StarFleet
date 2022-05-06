using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using Unity.Netcode;
using Core.ShipComponents;


struct PlayerControllerNetworkedInputs : INetworkSerializable
{
    public Vector2 Move; // 0, 0
    public Vector3 MouseWorldCoordiates; // 0,0,0
    public float YawDirection; // -1 or 1 or 0;
    public bool firing;

    // INetworkSerializable
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Move);
        serializer.SerializeValue(ref MouseWorldCoordiates);
        serializer.SerializeValue(ref YawDirection);
        serializer.SerializeValue(ref firing);

    }
    // ~INetworkSerializable
}

public class PlayerController : NetworkBehaviour
{
    #region Variables
    [Header("VARIABLES")]
    [SerializeField] public float velocity = 5f;
    [SerializeField] public float rotationSpeed = 5f;
    [SerializeField] public float yawRate;

    [Header("Ship")]
    [SerializeField]
    Chassis chassis;
    [Header("SHIP STATS")]

    [SerializeField] public float HP = 10f;
    [SerializeField] public float HPMax = 10f;
    [SerializeField] public float Shield = 10;
    [SerializeField] public float MaxShield = 10;

    [Header("INPUT VECTORS")]
    [SerializeField] Vector3 move;
    [SerializeField] Vector3 yaw;
    Rigidbody prb;

    [Header("GUI ASSETS")]
    [SerializeField] Slider hpslider;
    [SerializeField] Slider shieldslider;

    [Header("VFX Control")]
    [SerializeField] VisualEffect thrusterVFX;
    [SerializeField] VisualEffect thrusterSparksVFX;

    [Header("Ship Spawn Debug")]
    [SerializeField] GameObject ShipToSpawn;

    //
    //NetworkVariables
    PlayerControllerNetworkedInputs networkedInputs = new PlayerControllerNetworkedInputs();

    private DollyCamera2D dollyCamera;
    #endregion


    #region Start
    void Start()
    {
    }
    #endregion

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (NetworkObject.IsOwner)
        {
            RequestShipSpawnServerRpc();
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();
    }

    #region Update
    private void Update()
    {
        if (NetworkObject.NetworkManager.IsHost || NetworkObject.NetworkManager.IsServer)
        {
            SendNetworkedInputsToClientClientRpc(networkedInputs);
        }

        if (NetworkObject.IsOwner && chassis != null)
        {
            if (dollyCamera == null)
            {
                dollyCamera = Camera.main.GetComponent<DollyCamera2D>();
            }
            // Ensure Camera is always following the chassis
            dollyCamera.player = chassis.gameObject;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            if (this.NetworkObject.IsOwner == false)
            {
                // Do not process inputs because we are not the owner;
                return;
            }

            #region Get Inputs
            //Get inputs from WASD and turn into a Vector3 (x, y, z)
            move = new Vector3(-Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            //hpslider.value = HP / HPMax;
            //shieldslider.value = Shield / MaxShield;

            //Resets yawRate when not needing yaw, left to physics
            yawRate = 0f;

            //Get input for yawRate
            if (Input.GetKey(KeyCode.Q))
            {
                yawRate = 1f;
            }
            if (Input.GetKey(KeyCode.E))
            {
                yawRate = -1f;
            }
            #endregion


            #region Calculate Inputs
            //Set yaw to = yawRate in Vector3
            yaw = new Vector3(0, 0, yawRate);
            #endregion

            #region Update VFX

            #endregion

            networkedInputs.Move = move;
            networkedInputs.MouseWorldCoordiates = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            networkedInputs.YawDirection = yawRate;
            if (Input.GetMouseButton(0))
            {
                networkedInputs.firing = true;
            } else
            {
                networkedInputs.firing = false;
            }

            SendNetworkedInputsToServerServerRpc(networkedInputs);
        }

    }
    #endregion

    #region Take Damage and Die
    //Take damage on hit from enemy rounds
    public void TakeDamage(float damage)
    {
        HP -= damage;
        hpslider.value = HP / HPMax;

        if (HP <= 0)
        {
            Die();
        }
    }

    //Destroy ship when HP is 0
    void Die()
    {
        Destroy(gameObject);
    }
    #endregion

    #region FixedUpdate
    void FixedUpdate()
    {
        if (chassis == null)
        {
            return;
        }
        prb = chassis.GetRigidbody();
        if (NetworkObject.IsOwner)
        {
            #region Make Ship Move
            //Move ship based on inputs and rotation based on yaw 
            Debug.Log("Apply Thrust " + networkedInputs.Move.ToString());
            chassis.cpu.ApplyThrust(networkedInputs.Move);
            chassis.cpu.ApplyTorque(networkedInputs.YawDirection);
            #endregion

            #region Apply Space Brakes
            //Concistantly reset brakes when released.
            prb.drag = 0;
            prb.angularDrag = 0;


            //Apply Space Brakes.
            if (Input.GetKey(KeyCode.Space))
            {
                prb.drag = 1;
                prb.angularDrag = 1;
            }
            #endregion

            //Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            chassis.cpu.ApplyTurretRotation(networkedInputs.MouseWorldCoordiates);
        }

        if (networkedInputs.firing)
        {
            chassis.cpu.FireHardpoints();
        }

    }
    #endregion

    [ServerRpc(Delivery = RpcDelivery.Unreliable)]
    void SendNetworkedInputsToServerServerRpc(PlayerControllerNetworkedInputs inputs, ServerRpcParams serverRpcParams = default)
    {
        networkedInputs.Move = inputs.Move;
        networkedInputs.MouseWorldCoordiates = inputs.MouseWorldCoordiates;
        networkedInputs.YawDirection = inputs.YawDirection;
    }

    [ClientRpc(Delivery = RpcDelivery.Unreliable)]
    void SendNetworkedInputsToClientClientRpc(PlayerControllerNetworkedInputs inputs, ClientRpcParams clientrRpcParams = default)
    {
        networkedInputs.Move = inputs.Move;
        networkedInputs.MouseWorldCoordiates = inputs.MouseWorldCoordiates;
        networkedInputs.YawDirection = inputs.YawDirection;
    }

    [ServerRpc]
    void RequestShipSpawnServerRpc(ServerRpcParams serverRpcParams = default)
    {
        GameObject go = Instantiate(ShipToSpawn, transform.position, Quaternion.identity);
        //go.GetComponent<NetworkObject>().Spawn();
        //go.GetComponent<NetworkObject>().ChangeOwnership(serverRpcParams.Receive.SenderClientId);
        go.GetComponent<NetworkObject>().SpawnWithOwnership(serverRpcParams.Receive.SenderClientId);
        chassis = go.GetComponent<Chassis>();
        AquireShipClientRpc(serverRpcParams.Receive.SenderClientId, go.GetComponent<NetworkObject>().NetworkObjectId);
    }

    [ClientRpc]
    void AquireShipClientRpc(ulong clientId, ulong networkObjectId)
    {
        
        if (NetworkObject.NetworkManager.LocalClientId == clientId)
        {
            Debug.Log("Trying to aquire ship : " + networkObjectId + " : for clientId : " + clientId);
            NetworkObject[] potentials = FindObjectsOfType<NetworkObject>();
            for (int i = 0; i < potentials.Length; i += 1)
            {
                if (potentials[i].NetworkObjectId == networkObjectId)
                {
                    chassis = potentials[i].GetComponent<Chassis>();
                    Debug.Log("Successfully aquired ship");
                    return;
                }
            }
        }
        Debug.Log("Failed to aquire ship");
    }

}

//CODE BY GRVBBS & HERNANDEZ
//2022 MAGKORE GAME STUDIOS
