using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 100;


    private bool isDead;
    public Action<Health> OnDeath;

    void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

    public void HealHealth(int heal)
    {
        currentHealth += heal;
    }
}
