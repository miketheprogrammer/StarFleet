using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Core.ShipComponents
{
    public enum ShieldEmitterType
    {
        Bubble,
        Directional,
    }
    
    public class ShieldEmitter : ShipComponent
    {
        [SerializeField]
        public ShieldEmitterType type;

        [SerializeField]
        public float MaxShieldHP = 100;

        [SerializeField]
        public float CurrentShieldHP = 100;

        [SerializeField]
        [Tooltip("Time it takes for shield to fully regenerate")]
        public float RegenTime = 20;

        [SerializeField]
        public VisualEffect vfx;

        [SerializeField]
        public AnimationCurve damageMitigationCurve;


        public void OnEnable()
        {
            
        }

        public void Update()
        {
            gameObject.tag = chassis.tag;
            gameObject.layer = chassis.gameObject.layer;
            float shieldToRegen = Time.deltaTime * (MaxShieldHP / RegenTime);
            CurrentShieldHP = Mathf.Clamp(CurrentShieldHP + shieldToRegen, 0, MaxShieldHP);
            if (vfx != null)
            {
                vfx.SetFloat("PercentShield", CurrentShieldHP / MaxShieldHP);
            }
        }

        public void TakeDamage(float damage)
        {
            float shieldStrength = CurrentShieldHP / MaxShieldHP;
            float mitigation = damageMitigationCurve.Evaluate(1 - shieldStrength);
            Debug.Log("Shield Mitigated: " + mitigation + "%");

            float damageToShield = damage * mitigation;
            float damageNotMitigated = damage - damageToShield;
            CurrentShieldHP = Mathf.Clamp(CurrentShieldHP - damageToShield, 0, MaxShieldHP);
            chassis.TakeDamage(damageNotMitigated);
        }

    }
}

//CODE BY HERNANDEZ