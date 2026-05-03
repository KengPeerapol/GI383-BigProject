using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject victoryUI;
    public Health playerHealth;

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
}