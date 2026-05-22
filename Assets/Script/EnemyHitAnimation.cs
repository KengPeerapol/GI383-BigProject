using UnityEngine;

public class EnemyHitAnimation : MonoBehaviour
{
    [Header("Animation")]
    public Animator animator;
    public string getHitStateName = "get-hit";

    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayGetHit();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayGetHit();
        }
    }

    void PlayGetHit()
    {
        if (animator != null)
        {
            animator.Play(getHitStateName, 0, 0f);
            Debug.Log("Enemy เล่นอนิเมชั่น get-hit");
        }
        else
        {
            Debug.LogWarning("Enemy ไม่มี Animator");
        }
    }
}