using System;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;

    public float bulletLifeTime = 2f; // ⏱ เวลาก่อนกระสุนหาย (วินาที)

    public Ammo ammo;

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space) && ammo.currentAmmo > 1)
        {
            Shoot();
            ammo.currentAmmo = 0;
        }

        ammo.currentAmmo += 0.001f;
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = firePoint.up * bulletSpeed; // ⬆️ ยิงขึ้นด้านบน

        Destroy(bullet, bulletLifeTime); // 💥 ทำลายหลังผ่านเวลา
    }
    
}