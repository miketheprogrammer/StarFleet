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
        [Tooltip("Regeneration Rate / second")]
        public float RegenRate = 10;

        [SerializeField]
        public VisualEffect vfx;
        public void OnEnable()
        {
            
        }

        public void Update()
        {
            gameObject.tag = chassis.tag;
            gameObject.layer = chassis.gameObject.layer;
            float shieldToRegen = Time.deltaTime * RegenRate;
            CurrentShieldHP = Mathf.Clamp(CurrentShieldHP + shieldToRegen, 0, MaxShieldHP);
            if (vfx != null)
            {
                vfx.SetFloat("PercentShield", CurrentShieldHP / MaxShieldHP);
            }
        }

        public void TakeDamage(float damage)
        {
            CurrentShieldHP = Mathf.Clamp(CurrentShieldHP - damage, 0, MaxShieldHP);
        }

    }
}