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
        HardpointType hardpointType;

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

        
    }
}