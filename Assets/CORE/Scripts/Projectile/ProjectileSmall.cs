using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.ShipComponents;
public class ProjectileSmall : MonoBehaviour
{
    //Bullet
    [SerializeField] public float speed = 20;
    [SerializeField] public float damage = 1;
    public Rigidbody rb;





    private void Start()
    {
        rb.velocity = transform.right * speed;
    }



    //Bullet Force
    private void OnTriggerEnter(Collider hitInfo)
    {
        Debug.Log(hitInfo.name);

        oEnemyAI enemy;
        hitInfo.TryGetComponent<oEnemyAI>(out enemy);
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        ShieldEmitter shieldEmitter;
        hitInfo.TryGetComponent<ShieldEmitter>(out shieldEmitter);
        if (shieldEmitter != null)
        {
            // The shield emitter will delegate damage to the chassis
            shieldEmitter.TakeDamage(damage);
        }

        Destroy(gameObject);
    }









    //Gun Stats



    // Update is called once per frame
    void Update()
    {
        
    }
}
