using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A part of the CORE-SHIPCOMPONENTS class
namespace Core.ShipComponents
{

    //Component Type ShipComponent is MonoBehaviour
    public class ShipComponent : MonoBehaviour
    {
        public Chassis chassis;


        // EnsureReferences
        protected void Start()
        {
            EnsureReferences();
        }



        // EnsureReferences if chassis is empty and reset it to parent chassis
        protected void EnsureReferences()
        {
            if (chassis == null)
            {
                chassis = transform.parent.parent.gameObject.GetComponent<Chassis>();
            }
        }

        // EnsureReferences
        protected void Update()
        {
            EnsureReferences();
        }
    }
}

//CODE BY HERNANDEZ