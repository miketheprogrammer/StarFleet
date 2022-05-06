using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.ShipComponents
{
    public enum HardpointType
    {
        Turret,
        Fixed
    }
    public class Hardpoint : ShipComponent
    {
        [SerializeField]
        public HardpointType hardpointType;

        [Header("Hardpoint Barrel End")]
        [SerializeField] public Transform firePoint;
        [SerializeField] public GameObject bulletPrefab;
        [SerializeField] public GameObject playerObject;
        [SerializeField] public float reloadSpeed;
        [SerializeField] public float reloadTime = 10;
        [SerializeField] public float ammoCount = 20;

        public bool IsTurret()
        {
            return hardpointType == HardpointType.Turret;
        }
        public bool IsFixed()
        {
            return hardpointType == HardpointType.Fixed;
        }

        public HardpointType GetHardpointType()
        {
            return hardpointType;
        }

        //Spawn bullet on shoot reset reload to 0 and set bullets to ignore the collider input of choice. 
        public void Shoot()
        {
            if (playerObject == null)
            {
                playerObject = chassis.gameObject;
            }

            if (reloadSpeed < reloadTime)
            {
                return;
            }
            //Shooting Logic
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Physics.IgnoreCollision(bullet.GetComponent<Collider>(), playerObject.GetComponent<Collider>(), true);
            reloadSpeed = 0;
        }

        protected void Update()
        {
            base.Update();
            if (playerObject == null)
            {
                playerObject = chassis.gameObject;
            }
        }

        private void FixedUpdate()
        {

            //cooldown for reload
            if (reloadSpeed < reloadTime)
            {
                reloadSpeed = reloadSpeed + 1;
            }

        }

    }
}