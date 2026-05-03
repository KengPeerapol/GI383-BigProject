using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject victoryUI;
    public Health playerHealth;
    private Rigidbody2D rb;
    public float knockbackForce = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameOver();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            playerHealth.currentHealth -= 10;
            Transform enemy;
            
            ApplyKnockback(collision.transform);
            
            if (playerHealth.currentHealth <= 0)
            {
                GameOver();
            }
        }
        
        if(collision.CompareTag("Pickup"))
        {
            playerHealth.currentHealth += 10;
        }
        
        if (collision.CompareTag("Finish"))
        {
            Victory();
        }
    }

    void GameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0f; // ��ش��
    }

    void Victory()
    {
        victoryUI.SetActive(true);
        Time.timeScale = 0f;
    }
    
    void ApplyKnockback(Transform enemy)
    {
        Vector2 direction = (transform.position - enemy.position).normalized;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
    }
}