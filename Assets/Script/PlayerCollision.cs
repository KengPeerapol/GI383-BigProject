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

    [Header("Death Animation")]
    public string deathStateName = "died";
    public float deathDelay = 1.2f;
    public bool disablePlayerAfterDeath = true;

    private bool isDead = false;

    private Coroutine knockbackCoroutine;
    private Coroutine iframeCoroutine;

    private void Awake()
    {
        Time.timeScale = 1f;

        CameraMove.StartAllMove();

        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        if (victoryUI != null)
            victoryUI.SetActive(false);

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (movementScript == null)
            movementScript = GetComponent<PlayerCurveMovement>();

        if (movementScript != null)
        {
            movementScript.enabled = true;
            movementScript.isKnockback = false;
        }

        if (playerSprite == null)
            playerSprite = GetComponentInChildren<SpriteRenderer>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (rb != null)
        {
            rb.freezeRotation = true;
            rb.angularVelocity = 0f;

            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            rb.angularVelocity = 0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Enemy") && !isInvincible)
        {
            HandleHit(collision.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

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

        GameObject audioObj = GameObject.FindGameObjectWithTag("Sound");
        if (audioObj != null)
        {
            SoundManager audioManager = audioObj.GetComponent<SoundManager>();

            if (audioManager != null && audioManager.playerHit != null)
            {
                audioManager.PlaySFX(audioManager.playerHit);
            }
        }

        playerHealth.currentHealth -= damageAmount;

        if (playerHealth.currentHealth <= 0)
        {
            playerHealth.currentHealth = 0;

            if (!isDead)
            {
                StartCoroutine(DieRoutine());
            }

            return;
        }

        PlayHitAnimation();

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

    IEnumerator DieRoutine()
    {
        isDead = true;
        isInvincible = true;

        // หยุด Object ทุกตัวที่ใช้ CameraMove
        CameraMove.StopAllMove();

        // หยุด Coroutine เก่า
        if (knockbackCoroutine != null)
            StopCoroutine(knockbackCoroutine);

        if (iframeCoroutine != null)
            StopCoroutine(iframeCoroutine);

        // ปิดการขยับ Player
        if (movementScript != null)
            movementScript.enabled = false;

        // หยุดฟิสิกส์ Player
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // ปิด Collider กันชนซ้ำระหว่างตาย
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        // ตอนตายให้ตัวตรง
        transform.rotation = Quaternion.identity;

        // เล่น Animation died
        if (animator != null)
        {
            animator.ResetTrigger(hitTriggerName);
            animator.Play(deathStateName, 0, 0f);
            Debug.Log("เล่น Animation ตาย: " + deathStateName);
        }
        else
        {
            Debug.LogWarning("หา Animator ไม่เจอ");
        }

        // เล่นเสียงตาย
        GameObject audioObj = GameObject.FindGameObjectWithTag("Sound");
        if (audioObj != null)
        {
            SoundManager audioManager = audioObj.GetComponent<SoundManager>();

            if (audioManager != null)
            {
                audioManager.StopMusic();

                if (audioManager.death != null)
                    audioManager.PlaySFX(audioManager.death);
            }
        }

        // รอให้ Animation died เล่นจบก่อน
        yield return new WaitForSeconds(deathDelay);

        // ซ่อน Player ทันทีหลัง Animation จบ
        HidePlayerImmediately();

        // เปิด Game Over หลัง Player หาย
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
        else
        {
            Debug.LogWarning("ยังไม่ได้ลาก GameOver UI ใส่ใน Inspector");
        }

        // หยุดเวลาเกม
        Time.timeScale = 0f;

        // ปิดตัว Player แทนการลบ
        // สำคัญ: ต้องเป็นบรรทัดท้าย ๆ เพราะถ้าปิดก่อน โค้ดหลังจากนี้จะไม่ทำงาน
        if (disablePlayerAfterDeath)
        {
            gameObject.SetActive(false);
        }
    }

    void HidePlayerImmediately()
    {
        // ปิดภาพทุก Sprite ของ Player และลูก ๆ
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in sprites)
        {
            sr.enabled = false;
        }

        // ปิด Animator กันภาพค้าง
        if (animator != null)
        {
            animator.enabled = false;
        }

        // ปิด Collider ทุกตัว
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        // หยุด Rigidbody
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false;
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

        // ถ้ามี maxHealth ค่อยเปิดใช้
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

            Vector2 direction = ((Vector2)transform.position - (Vector2)enemyPos).normalized;

            direction.y += yRepositionAmount;
            direction = direction.normalized;

            Vector2 knockbackVelocity = direction * bounceForce;

            float timer = 0f;

            while (timer < knockbackDuration)
            {
                rb.linearVelocity = knockbackVelocity;
                rb.angularVelocity = 0f;

                timer += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

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

        yield return new WaitForSeconds(iFrameDuration);

        isInvincible = false;
    }

    void GameOver()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    void Victory()
    {
        CameraMove.StopAllMove();

        GameObject audioObj = GameObject.FindGameObjectWithTag("Sound");
        if (audioObj != null)
        {
            SoundManager audioManager = audioObj.GetComponent<SoundManager>();

            if (audioManager != null)
            {
                audioManager.StopMusic();

                if (audioManager.victory != null)
                    audioManager.PlaySFX(audioManager.victory);
            }
        }

        if (victoryUI != null)
        {
            victoryUI.SetActive(true);
        }

        Time.timeScale = 0f;
    }
}