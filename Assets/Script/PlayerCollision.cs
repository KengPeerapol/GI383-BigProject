using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject victoryUI;
    public Health playerHealth;
    public SpriteRenderer playerSprite; // Drag your SpriteRenderer here
    
    public PlayerCurveMovement movementScript;
    public Rigidbody2D rb;
    
    public float bounceForce = 15f;
    public float knockbackDuration = 0.2f;
    public float yRepositionAmount = 1.5f; // Extra "pop" on the Y axis
    
    public float iFrameDuration = 1.5f;
    private bool isInvincible = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isInvincible)
        {
            HandleHit(collision.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !isInvincible)
        {
            HandleHit(collision.transform.position);
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

    void HandleHit(Vector3 enemyPos)
    {
        playerHealth.currentHealth -= 10;
        
        if (playerHealth.currentHealth <= 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(KnockbackRoutine(enemyPos));
            StartCoroutine(IFrameRoutine());
        }
    }

    IEnumerator KnockbackRoutine(Vector3 enemyPos)
    {
        movementScript.isKnockback = true;
        
        rb.linearVelocity = Vector2.zero;
        
        Vector2 direction = (transform.position - enemyPos).normalized;
        direction.y += yRepositionAmount; 

        rb.AddForce(direction.normalized * bounceForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        movementScript.isKnockback = false;
    }

    IEnumerator IFrameRoutine()
    {
        isInvincible = true;

        float timer = 0;
        while (timer < iFrameDuration)
        {
            playerSprite.enabled = !playerSprite.enabled; 
            yield return new WaitForSeconds(0.1f); 
            timer += 0.1f;
        }

        playerSprite.enabled = true; 
        isInvincible = false;
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