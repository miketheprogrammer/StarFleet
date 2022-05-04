using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class oEnemyAI : MonoBehaviour
{
    #region Variables
    [Header("BASIC STATS")]
    [SerializeField] public float HP = 10;
    [SerializeField] public float MaxHP = 10;
    [SerializeField] public float Shield = 10;
    [SerializeField] public float MaxShield = 10;

    [Header("ENEMY UI FOR HP AND SHIELD")]
    [SerializeField] Slider slider;
    [SerializeField] Slider shieldslider;

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