using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.ShipComponents
{
    public class CentralProcessingUnit : ShipComponent
    {
        Chassis chassis;

        
        public void Start()
        {
            chassis = transform.parent.parent.gameObject.GetComponent<Chassis>();
        }

        private void EnsureReferences()
        {

        }

        public void ReceiveMessage()
        {
            
        }

        public void ApplyThrust(Vector2 move)
        {
            Debug.Log("_________________________________________");
            chassis.thrusters.FindAll((Thruster t) => t.thrusterType != ThrusterType.Rotational).ForEach((Thruster thruster) =>
            {
                thruster.Activate(move);
            });
        }

        public void ApplyTorque(float yawDirection)
        {
            chassis.thrusters.FindAll((Thruster t) => t.thrusterType == ThrusterType.Rotational).ForEach((Thruster thruster) =>
            {
                thruster.Activate(yawDirection);
            });
        }
    }
}