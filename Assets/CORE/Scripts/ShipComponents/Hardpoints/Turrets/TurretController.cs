using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//A part of the CORE-SHIPCOMPONENTS class
namespace Core.ShipComponents
{

    //Component Type TurretController is Hardpoint
    public class TurretController : Hardpoint
    {
        [Header("Turret Rotation")]
        [SerializeField] public float rotationSpeed = 5f;

        public void Rotate(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        }
    }
}

//CODE BY GRVBBS & HERNANDEZ
