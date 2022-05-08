using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using FMODUnity;



//A part of the CORE-SHIPCOMPONENTS class
namespace Core.ShipComponents
{

    //Types of thrusters
    public enum ThrusterType
    {
        Forward,
        Reverse,
        ManeuveringStarboard,
        ManeuveringPort,
        Rotational,
    }


    //Component Type Thruster is ShipComponent
    public class Thruster : ShipComponent
    {
        //Get Rigid Body of Ship
        public Rigidbody rb;
        //Type of thruster
        [SerializeField]
        public ThrusterType thrusterType = ThrusterType.Forward;
        //How much force
        [SerializeField]
        float force = 5f;
        //Burn Modifier
        [SerializeField]
        float burnModifier = 10f;
        //Yaw Force
        [SerializeField]
        float yawForce = 20f;
        //VFX for thruster at max input
        [SerializeField]
        [Range(0,1)]
        float vfxIntensity = .5f;
        //VFX for thruster at min input
        [SerializeField]
        [Range(0,1)]
        float vfxLowIntensity = 0f;


        //Types of VFX for thruster
        [Header("VFX Control")]
        [SerializeField] 
        VisualEffect thrusterVFX;
        [SerializeField] 
        VisualEffect thrusterSparksVFX;

        //Thruster Sound Effects
        [Header("SFX Control")]
        [SerializeField] 
        FMODUnity.StudioEventEmitter thrusterSFX;

        //thruster on or off
        [SerializeField] 
        [Range(0,1)] 
        float thrusterSFXIntensity = 0f;

        //private Vector3 Yaw = new Vector3(0, 0, 1);



        //Ensure References function
        public void EnsureReferences()
        {
            //if chassis is null set the chassis to the parents chassis type
            if (chassis == null)
            {
                chassis = transform.parent.parent.gameObject.GetComponent<Chassis>();
            }

            //if rigid body is null and chassis is not null set rigid body to chassis rigid body
            if (rb == null)
            {
                if (chassis != null)
                {
                    rb = chassis.GetRigidbody();
                }
            }
        }


        //Update 
        private void Update()
        {
            EnsureReferences();
        }


        //Get Direction Function
        private Vector3 GetDirection()
        {

            //get input value based on direction of thruster
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

            //reset
            return Vector3.zero;
        }

        //Activate VFX function
        private void ActivateVFX()
        {
            //Debug.Log("Activatign Thruster VFX : " + gameObject.name);
            thrusterVFX.SetFloat("Intensity", vfxIntensity);
            if (thrusterSFX != null)
            {
                thrusterSFX.SetParameter("Intensity", thrusterSFXIntensity);
            }
        }

        //Deactivate VFX function
        private void DeactivateVFX()
        {
           // Debug.Log("Deactivatign Thruster VFX : " + gameObject.name);
            thrusterVFX.SetFloat("Intensity", vfxLowIntensity);
            if (thrusterSFX != null)
            {
                thrusterSFX.SetParameter("Intensity", 0f);
            }
        }

        //Activate thruster based on the object based on force
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


        //Activate yaw thruster when yaw input is applied
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