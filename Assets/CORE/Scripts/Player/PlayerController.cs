using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("VARIABLES")]
    [SerializeField] public float velocity = 5f;
    [SerializeField] public float rotationSpeed = 5f;
    [SerializeField] public float yawRate;

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
    #endregion


    #region Start
    void Start()
    {
        prb = GetComponent<Rigidbody>();
        prb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
        prb.useGravity = false;
        Camera.main.GetComponent<DollyCamera2D>().player = this.gameObject;
    }
    #endregion

    #region Update
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
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
        if (Input.GetAxisRaw("Vertical") > 0.1)
        {
            thrusterVFX.SetFloat("Intensity", .5f);
            thrusterSparksVFX.enabled = true;
        } 
        else
        {
            thrusterVFX.SetFloat("Intensity", 0f);
            thrusterSparksVFX.enabled = false;
        }
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
        #region Make Ship Move
        //Move ship based on inputs and rotation based on yaw 
        prb.AddRelativeForce(move.y * Vector3.right * velocity * Time.deltaTime);
        prb.AddRelativeForce(move.x * Vector3.up * velocity * Time.deltaTime);
        prb.AddTorque(yaw.z * Vector3.forward * rotationSpeed * Time.deltaTime);
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
    }
    #endregion
}

//CODE BY GRVBBS & HERNANDEZ
//2022 MAGKORE GAME STUDIOS
