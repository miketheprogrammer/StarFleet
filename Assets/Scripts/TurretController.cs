using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    #region Variables
    [Header("Turret Rotation")]
    [SerializeField] public float rotationSpeed = 5f;

    [Header("Turret Barrel End")]
    [SerializeField] public Transform firePoint;
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public GameObject playerObject;
    [SerializeField] public float reloadSpeed;
    [SerializeField] public float reloadTime = 10;
    [SerializeField] public float ammoCount = 20;
    #endregion


    // Update is called once per frame
    void Update()
    {
        //Vector
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);


    }

    private void FixedUpdate()
    {

        //cooldown for reload
        if (reloadSpeed < reloadTime)
        {
            reloadSpeed = reloadSpeed + 1;
        }

        //Reload check then fire
        if (Input.GetMouseButton(0))
        {
            if (reloadSpeed >= reloadTime)
            {
                Shoot();
            } 
        }
    }


    //Spawn bullet on shoot reset reload to 0 and set bullets to ignore the collider input of choice. 
    void Shoot()
    {
        //Shooting Logic
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Physics.IgnoreCollision(bullet.GetComponent<Collider>(), playerObject.GetComponent<Collider>(), true);
        reloadSpeed = 0;
    }
}

//CODE BY GRVBBS & HERNANDEZ
//2022 MAGKORE GAME STUDIOS