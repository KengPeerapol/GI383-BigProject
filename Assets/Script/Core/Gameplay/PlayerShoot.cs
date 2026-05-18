using System;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Bullet")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float bulletLifeTime = 2f; // เวลาก่อนกระสุนหาย

    [Header("Ammo")]
    public Ammo ammo;
    public float reloadSpeed = 0.001f;
    public float needAmmoToShoot = 1f;

    [Header("Animation")]
    public Animator animator;
    public string shootTriggerName = "Shoot";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && ammo.currentAmmo > needAmmoToShoot)
        {
            Shoot();

            // เล่นอนิเมชั่นยิง
            if (animator != null)
            {
                animator.SetTrigger(shootTriggerName);
            }

            ammo.currentAmmo = 0;
        }

        ammo.currentAmmo += reloadSpeed;
    }

    void Shoot()
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("ยังไม่ได้ใส่ bulletPrefab");
            return;
        }

        if (firePoint == null)
        {
            Debug.LogWarning("ยังไม่ได้ใส่ firePoint");
            return;
        }

        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // ยิงขึ้นด้านบนตาม firePoint
            rb.linearVelocity = firePoint.up * bulletSpeed;
        }
        else
        {
            Debug.LogWarning("Bullet ไม่มี Rigidbody2D");
        }

        Destroy(bullet, bulletLifeTime);
    }
}