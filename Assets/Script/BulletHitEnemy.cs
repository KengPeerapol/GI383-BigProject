using UnityEngine;

public class BulletHitEnemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyDeath enemyDeath = other.GetComponent<EnemyDeath>();

            if (enemyDeath == null)
                enemyDeath = other.GetComponentInParent<EnemyDeath>();

            if (enemyDeath != null)
            {
                enemyDeath.Die();
            }
            else
            {
                Debug.LogWarning("Enemy ไม่มีสคริปต์ EnemyDeath");
            }

            Destroy(gameObject); // ลบกระสุน
        }
    }
}