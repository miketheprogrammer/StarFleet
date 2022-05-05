using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class oEnemyAI : MonoBehaviour
{
    #region Variables

    [Header("VARIABLES")]
    [SerializeField] public float velocity = 25;
    [SerializeField] public float rotationSpeed = 15;

    [Header("BASIC STATS")]
    [SerializeField] public float HP = 10;
    [SerializeField] public float MaxHP = 10;
    [SerializeField] public float Shield = 10;
    [SerializeField] public float MaxShield = 10;

    [Header("ENEMY UI FOR HP AND SHIELD")]
    [SerializeField] Slider slider;
    [SerializeField] Slider shieldslider;

    [Header("LOGIC STATE")]
    [SerializeField] public bool waitingState = false;
    [SerializeField] public bool roamingState = false;
    [SerializeField] public bool attackingState = false;
    [SerializeField] public float timerClock = 0;
    [SerializeField] public float resetTime = 60;

    [Header("ENEMY AI MOVEMENT VARIABLES")]
    [SerializeField] Vector3 startingPosition;
    [SerializeField] Vector3 roamingPosition;
    [SerializeField] Vector3 targetPosition;
    [SerializeField] Rigidbody erb;
    #endregion

    #region Enemy AI Movement
    //Get starting position and roaming position/target pos

    private void Start()
    {
        startingPosition = transform.position;        
    }

    public void FixedUpdate()
    {
        timerClock += 1;
        waitingState = true;
        roamingState = false;
        erb.drag = .5f;
        erb.angularDrag = .5f;



        if (timerClock >= resetTime)
        {
            roamingPosition = GetRoamingPosition();
            roamingState = true;
            waitingState = false;
            timerClock = 0;
            erb.drag = 0;
            erb.angularDrag = 0;
            //Move ship based on inputs and rotation based on yaw 


            erb.AddRelativeForce(roamingPosition.x * Vector3.right * velocity * Time.deltaTime);
            erb.AddRelativeForce(roamingPosition.y * Vector3.up * velocity * Time.deltaTime);
            erb.AddTorque(Random.Range(-1f, 1f) * Vector3.forward * rotationSpeed * Time.deltaTime);

        }


    }
    private Vector3 GetRoamingPosition()
    {
    return startingPosition + GetRandomDir() * Random.Range(10f, 70f);
    }

    public static Vector3 GetRandomDir()
    {
    return new Vector3(UnityEngine.Random.Range(1f, 5f), UnityEngine.Random.Range(-1f, 1f)).normalized;
    }

    #endregion

    //Hp and Shield Slider
    #region HP and Shield
    public void Update()
    {
        slider.value = HP / MaxHP;
        shieldslider.value = Shield / MaxShield;
    }
    #endregion


    #region Take damage and Die

 
    //Take damage
    public void TakeDamage (float damage)
    {
        HP -= damage;
        slider.value = HP/MaxHP;

        if (HP <= 0)
        {
            Die();
        }
    }
    //Die
    void Die()
    {
        Destroy(gameObject);
    }
    #endregion

}


//CODE BY GRVBBS & HERNANDEZ
//2022 MAGKORE GAME STUDIOS