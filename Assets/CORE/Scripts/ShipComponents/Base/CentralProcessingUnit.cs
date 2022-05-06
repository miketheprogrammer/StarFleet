using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.ShipComponents
{
    public class CentralProcessingUnit : ShipComponent
    {

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

        public void ApplyTurretRotation(Vector2 direction)
        {
            chassis.hardpoints.FindAll((Hardpoint h) => h.hardpointType == HardpointType.Turret).ForEach((Hardpoint h) =>
            {
               (h as TurretController).Rotate(direction);
            });
        }

        public void FireHardpoints()
        {
            chassis.hardpoints.ForEach((Hardpoint h) =>
            {
                h.Shoot();
            });
        }
    }
}