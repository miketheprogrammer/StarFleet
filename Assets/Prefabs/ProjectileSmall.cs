using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        oEnemyAI enemy = hitInfo.GetComponent<oEnemyAI>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }









    //Gun Stats



    // Update is called once per frame
    void Update()
    {
        
    }
}
