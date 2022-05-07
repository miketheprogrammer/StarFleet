using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
namespace Core.ShipComponents
{
    public class Chassis : ShipComponent
    {
        public Rigidbody rb;

        public GameObject ComponentsContainer;

        public List<Hardpoint> hardpoints;
        public CentralProcessingUnit cpu;
        public QuantumDrive quantumDrive;
        public List<ShieldEmitter> shieldEmitters;
        public TargetingComputer targetingComputer;
        public List<Thruster> thrusters;

        private NetworkObject networkObject;

        [Header("SHIP STATS")]

        [SerializeField] public float HP = 100f;
        [SerializeField] public float HPMax = 100f;

        [Header("SHIP STATS -- DO NOT MODIFY")]
        [SerializeField] private float Shield = 10;
        [SerializeField] private float MaxShield = 10;

        public PlayerController pc;


        public GameObject Explosion;
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

            networkObject = GetComponent<NetworkObject>();
        }

        // Update is called once per frame
        void Update()
        {
            // Do not call the base

            if (Input.GetKeyDown(KeyCode.R))
            {
                //   chassis.gameObject.SetActive(true);
                Resurrect();
            }
        }

        public void TakeDamage(float damage)
        {
            HP -= damage;
            Debug.Log("Chassis took : " + damage + " damage, and has " + HP + " left");
            //hpslider.value = HP / HPMax;

            if (HP <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            if (Explosion != null)
            {
                Explosion.SetActive(true);
            }
            //gameObject.SetActive(false);
            GetComponent<SpriteRenderer>().enabled = false;
            ComponentsContainer.SetActive(false);

            //Destroy(chassis.gameObject);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        public void Resurrect()
        {
            GetComponent<SpriteRenderer>().enabled = true;
            ComponentsContainer.SetActive(true);
            Explosion.SetActive(false);
            Repair();
        }

        public void Repair()
        {
            HP = HPMax;
        }

        public Rigidbody GetRigidbody()
        {
            //Debug.Log("Trying to fetch rb from chassis");
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