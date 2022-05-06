using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.ShipComponents
{
    public class TurretController : Hardpoint
    {
        #region Variables
        [Header("Turret Rotation")]
        [SerializeField] public float rotationSpeed = 5f;

        public void Rotate(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

        }
        #endregion
    }
}

//CODE BY GRVBBS & HERNANDEZ
//2022 MAGKORE GAME STUDIOS