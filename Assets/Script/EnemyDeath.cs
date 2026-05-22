using System.Collections;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    [Header("Animation")]
    public Animator animator;
    public string diedStateName = "died";

    private bool isDead = false;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        StartCoroutine(DieRoutine());
    }

    private IEnumerator DieRoutine()
    {
        Debug.Log("Enemy เล่น died");

        // ปิด Collider ทุกตัวใน Enemy กันโดนซ้ำ
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        // หยุด Rigidbody ถ้ามี
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        float animLength = 0.5f;

        if (animator != null)
        {
            animator.Play(diedStateName, 0, 0f);

            // หาเวลาจริงของ animation clip ชื่อ died
            RuntimeAnimatorController controller = animator.runtimeAnimatorController;

            foreach (AnimationClip clip in controller.animationClips)
            {
                if (clip.name == diedStateName)
                {
                    animLength = clip.length;
                    break;
                }
            }
        }
        else
        {
            Debug.LogWarning("Enemy ไม่มี Animator");
        }

        yield return new WaitForSeconds(animLength);

        Debug.Log("ลบ Enemy หลัง animation died จบ");

        Destroy(gameObject);
    }
}