using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
namespace Core.ShipComponents
{
    public class Chassis : ShipComponent
    {
        public Rigidbody rb;

        public List<Hardpoint> hardpoints;
        public CentralProcessingUnit cpu;
        public QuantumDrive quantumDrive;
        public List<ShieldEmitter> shieldEmitters;
        public TargetingComputer targetingComputer;
        public List<Thruster> thrusters;

        private NetworkObject networkObject;
        // Start is called before the first frame update
        void Start()
        {
            if (rb == null)
            {
                rb = GetComponent<Rigidbody>();
                rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
                rb.useGravity = false;
            }
            rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
            rb.useGravity = false;
        }

        // Update is called once per frame
        void Update()
        {
            // Do not call the base
        }

        public Rigidbody GetRigidbody()
        {
            Debug.Log("Trying to fetch rb from chassis");
            return rb;
        }

        public bool isOwner()
        {
            return networkObject.IsOwner;
        }

        public bool isOwnedByServer()
        {
            return networkObject.IsOwnedByServer;
        }
    }
}