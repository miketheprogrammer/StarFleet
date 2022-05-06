using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Core.ShipComponents
{
    public enum ThrusterType
    {
        Forward,
        Reverse,
        ManeuveringStarboard,
        ManeuveringPort,
        Rotational,
    }
    public class Thruster : ShipComponent
    {
        public Rigidbody rb;

        [SerializeField]
        public ThrusterType thrusterType = ThrusterType.Forward;
        [SerializeField]
        float force = 5f;
        [SerializeField]
        float burnModifier = 10f;
        [SerializeField]
        float yawForce = 20f;
        [SerializeField]
        [Range(0,1)]
        float vfxIntensity = .5f;
        [SerializeField]
        [Range(0,1)]
        float vfxLowIntensity = 0f;

        [Header("VFX Control")]
        [SerializeField] VisualEffect thrusterVFX;
        [SerializeField] VisualEffect thrusterSparksVFX;

        //private Vector3 Yaw = new Vector3(0, 0, 1);

        public void EnsureReferences()
        {
            if (chassis == null)
            {
                chassis = transform.parent.parent.gameObject.GetComponent<Chassis>();
            }

            if (rb == null)
            {
                if (chassis != null)
                {
                    rb = chassis.GetRigidbody();
                }
            }
        }

        private void Update()
        {
            EnsureReferences();
        }

        private Vector3 GetDirection()
        {
            if (thrusterType == ThrusterType.Forward)
            {
                return Vector3.right;
            }
            if (thrusterType == ThrusterType.Reverse)
            {
                return Vector3.right;
            }
            if (thrusterType == ThrusterType.ManeuveringPort)
            {
                return Vector3.up;
            }
            if (thrusterType == ThrusterType.ManeuveringStarboard)
            {
                return Vector3.up;
            }

            return Vector3.zero;
        }

        private void ActivateVFX()
        {
            //Debug.Log("Activatign Thruster VFX : " + gameObject.name);
            thrusterVFX.SetFloat("Intensity", vfxIntensity);
        }
        private void DeactivateVFX()
        {
           // Debug.Log("Deactivatign Thruster VFX : " + gameObject.name);
            thrusterVFX.SetFloat("Intensity", vfxLowIntensity);
        }

        public void Activate(Vector2 move)
        {
            EnsureReferences();
            if (rb != null)
            {
                if (thrusterType != ThrusterType.Rotational)
                {
                    if (thrusterType == ThrusterType.Forward)
                    {
                        if (move.y > 0.01f)
                        {
                            //Debug.Log("Adding Relative Force: " + move.y * GetDirection() * force * Time.deltaTime);
                            rb.AddRelativeForce(move.y * GetDirection() * force * Time.deltaTime);
                            ActivateVFX();
                        }
                        else
                        {
                            DeactivateVFX();
                        }
                    }
                    if (thrusterType == ThrusterType.Reverse)
                    {
                        if (move.y < -0.01f)
                        {
                            rb.AddRelativeForce(move.y * GetDirection() * force * Time.deltaTime);
                            ActivateVFX();
                        } 
                        else
                        {
                            DeactivateVFX();
                        }
                        
                        
                    }
                    if (thrusterType == ThrusterType.ManeuveringStarboard)
                    {
                        if (move.x > 0.01f)
                        {
                            rb.AddRelativeForce(move.x * GetDirection() * force * Time.deltaTime);
                            ActivateVFX();
                        }
                        else
                        {
                            DeactivateVFX();
                        }
                    }
                    if (thrusterType == ThrusterType.ManeuveringPort)
                    {
                        if (move.x < -0.01f)
                        {
                            rb.AddRelativeForce(move.x * GetDirection() * force * Time.deltaTime);
                            ActivateVFX();
                        }
                        else
                        {
                            DeactivateVFX();
                        }
                    }
                    
                }
                
            }
        }

        public void Activate(float yawDirection)
        {
            EnsureReferences();
            if (rb != null)
            {
                if (thrusterType == ThrusterType.Rotational)
                {
                    rb.AddTorque(Vector3.forward * yawDirection * yawForce * Time.deltaTime);
                }

            }
        }
    }
}