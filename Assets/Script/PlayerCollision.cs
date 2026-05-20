using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    [Header("UI")]
    public GameObject gameOverUI;
    public GameObject victoryUI;

    [Header("Health")]
    public Health playerHealth;
    public int damageAmount = 10;
    public int healAmount = 10;

    [Header("Sprite")]
    public SpriteRenderer playerSprite;

    [Header("Movement / Physics")]
    public PlayerCurveMovement movementScript;
    public Rigidbody2D rb;

    [Header("Knockback")]
    public float bounceForce = 15f;
    public float knockbackDuration = 0.2f;
    public float yRepositionAmount = 1.5f;

    [Header("Invincible")]
    public float iFrameDuration = 1.5f;
    private bool isInvincible = false;

    [Header("Animation")]
    public Animator animator;
    public string hitTriggerName = "Hit";

    private Coroutine knockbackCoroutine;
    private Coroutine iframeCoroutine;

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (movementScript == null)
            movementScript = GetComponent<PlayerCurveMovement>();

        if (playerSprite == null)
            playerSprite = GetComponentInChildren<SpriteRenderer>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (rb != null)
        {
            // กันตัวเอียงเวลาโดนชน
            rb.freezeRotation = true;
            rb.angularVelocity = 0f;
            rb.rotation = 0f;

            // ช่วยลดอาการภาพสั่น/เบลอตอนชน
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
    }

    private void FixedUpdate()
    {
        // กันตัวเอียงหรือหมุนค้างจนดูสั่น
        if (rb != null)
        {
            rb.angularVelocity = 0f;
            rb.rotation = 0f;
        }

        transform.rotation = Quaternion.identity;
    }

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

        if (collision.CompareTag("Pickup"))
        {
            HealPlayer();

            // ถ้าอยากให้ของหายหลังเก็บ ให้เปิดบรรทัดนี้
            // Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Finish"))
        {
            Victory();
        }
    }

    void HandleHit(Vector3 enemyPos)
    {
        if (playerHealth == null)
        {
            Debug.LogWarning("ยังไม่ได้ลาก Player Health ใส่ใน Inspector");
            return;
        }

        playerHealth.currentHealth -= damageAmount;

        PlayHitAnimation();

        if (playerHealth.currentHealth <= 0)
        {
            playerHealth.currentHealth = 0;
            GameOver();
            return;
        }

        // กัน Coroutine ซ้อนกันหลายอันจนตัวสั่น/เบลอ
        if (knockbackCoroutine != null)
            StopCoroutine(knockbackCoroutine);

        if (iframeCoroutine != null)
            StopCoroutine(iframeCoroutine);

        knockbackCoroutine = StartCoroutine(KnockbackRoutine(enemyPos));
        iframeCoroutine = StartCoroutine(IFrameRoutine());
    }

    void PlayHitAnimation()
    {
        if (animator != null)
        {
            animator.ResetTrigger(hitTriggerName);
            animator.SetTrigger(hitTriggerName);
        }
        else
        {
            Debug.LogWarning("ยังไม่ได้ลาก Animator หรือหา Animator ไม่เจอ");
        }
    }

    void HealPlayer()
    {
        if (playerHealth == null)
        {
            Debug.LogWarning("ยังไม่ได้ลาก Player Health ใส่ใน Inspector");
            return;
        }

        playerHealth.currentHealth += healAmount;

        // ถ้า Health ของคุณมี maxHealth ค่อยเปิดบรรทัดนี้
        // playerHealth.currentHealth = Mathf.Clamp(playerHealth.currentHealth, 0, playerHealth.maxHealth);
    }

    IEnumerator KnockbackRoutine(Vector3 enemyPos)
    {
        if (movementScript != null)
        {
            movementScript.isKnockback = true;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.rotation = 0f;
            transform.rotation = Quaternion.identity;

            Vector2 direction = ((Vector2)transform.position - (Vector2)enemyPos).normalized;

            // ใช้ค่าจากตัวแปรเดิม ไม่ต้องเปลี่ยนค่าใน Inspector
            direction.y += yRepositionAmount;
            direction = direction.normalized;

            Vector2 knockbackVelocity = direction * bounceForce;

            float timer = 0f;

            while (timer < knockbackDuration)
            {
                rb.linearVelocity = knockbackVelocity;

                // กันหมุน/เอียงระหว่างเด้ง
                rb.angularVelocity = 0f;
                rb.rotation = 0f;
                transform.rotation = Quaternion.identity;

                timer += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            // หยุดแรงหลัง Knockback จบ ไม่ให้ไหลจนดูเบลอ
            rb.linearVelocity = Vector2.zero;
        }

        if (movementScript != null)
        {
            movementScript.isKnockback = false;
        }
    }

    IEnumerator IFrameRoutine()
    {
        isInvincible = true;

        // กันโดนดาเมจซ้ำเฉย ๆ
        // ไม่มีล่องหน ไม่มีจาง ไม่กระพริบ
        yield return new WaitForSeconds(iFrameDuration);

        isInvincible = false;
    }

    void GameOver()
    {
        // 1. ค้นหา Object ที่มี Tag "Sound" (แก้ให้ตรงกับใน Unity ของคุณแล้ว)
        GameObject audioObj = GameObject.FindGameObjectWithTag("Sound");

        if (audioObj != null)
        {
            SoundManager audioManager = audioObj.GetComponent<SoundManager>();
            if (audioManager != null)
            {
                audioManager.StopMusic(); // สั่งหยุดเพลง Background
                audioManager.PlaySFX(audioManager.death); // สั่งเล่นเสียงตอนตาย
            }
        }
        else
        {
            Debug.LogWarning("หา Tag 'Sound' ไม่เจอ! เช็กให้ชัวร์ว่าพิมพ์พิมพ์ใหญ่-เล็กตรงกัน");
        }

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    void Victory()
    {
        if (victoryUI != null)
        {
            victoryUI.SetActive(true);
        }

        Time.timeScale = 0f;
    }
}