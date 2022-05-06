using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using Unity.Netcode;
using Core.ShipComponents;
public class PlayerController : NetworkBehaviour
{
    #region Variables
    [Header("VARIABLES")]
    [SerializeField] public float velocity = 5f;
    [SerializeField] public float rotationSpeed = 5f;
    [SerializeField] public float yawRate;

    [Header("Ship")]
    [SerializeField]
    Chassis chassis;
    [Header("SHIP STATS")]

    [SerializeField] public float HP = 10f;
    [SerializeField] public float HPMax = 10f;
    [SerializeField] public float Shield = 10;
    [SerializeField] public float MaxShield = 10;

    [Header("INPUT VECTORS")]
    [SerializeField] Vector3 move;
    [SerializeField] Vector3 yaw;
    Rigidbody prb;

    [Header("GUI ASSETS")]
    [SerializeField] Slider hpslider;
    [SerializeField] Slider shieldslider;

    [Header("VFX Control")]
    [SerializeField] VisualEffect thrusterVFX;
    [SerializeField] VisualEffect thrusterSparksVFX;

    private DollyCamera2D dollyCamera;
    #endregion


    #region Start
    void Start()
    {
    }
    #endregion

    #region Update
    private void Update()
    {
        if (dollyCamera == null)
        {
            dollyCamera = Camera.main.GetComponent<DollyCamera2D>();
        }
        // Ensure Camera is always following the chassis
        dollyCamera.player = chassis.gameObject;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (this.NetworkObject.IsOwner == false)
        {
            // Do not process inputs because we are not the owner;
            return;
        }

        #region Get Inputs
        //Get inputs from WASD and turn into a Vector3 (x, y, z)
        move = new Vector3(-Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        hpslider.value = HP / HPMax;
        shieldslider.value = Shield / MaxShield;

        //Resets yawRate when not needing yaw, left to physics
        yawRate = 0f;

        //Get input for yawRate
        if (Input.GetKey(KeyCode.Q))
        {
            yawRate = 1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            yawRate = -1f;
        }
        #endregion


        #region Calculate Inputs
        //Set yaw to = yawRate in Vector3
        yaw = new Vector3(0, 0, yawRate);
        #endregion

        #region Update VFX

        #endregion

    }
    #endregion

    #region Take Damage and Die
    //Take damage on hit from enemy rounds
    public void TakeDamage(float damage)
    {
        HP -= damage;
        hpslider.value = HP / HPMax;

        if (HP <= 0)
        {
            Die();
        }
    }

    //Destroy ship when HP is 0
    void Die()
    {
        Destroy(gameObject);
    }
    #endregion

    #region FixedUpdate
    void FixedUpdate()
    {
        prb = chassis.GetRigidbody();
        #region Make Ship Move
        //Move ship based on inputs and rotation based on yaw 
        chassis.cpu.ApplyThrust(move);
        chassis.cpu.ApplyTorque(yaw.z);
        #endregion

        #region Apply Space Brakes
        //Concistantly reset brakes when released.
        prb.drag = 0;
        prb.angularDrag = 0;


        //Apply Space Brakes.
        if (Input.GetKey(KeyCode.Space))
        {
            prb.drag = 1;
            prb.angularDrag = 1;
        }
        #endregion

        //Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        chassis.cpu.ApplyTurretRotation(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (Input.GetMouseButton(0))
        {
            chassis.cpu.FireHardpoints();
        }
        
    }
    #endregion
}

//CODE BY GRVBBS & HERNANDEZ
//2022 MAGKORE GAME STUDIOS
