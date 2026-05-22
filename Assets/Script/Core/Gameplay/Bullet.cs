using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject hitVfx;

    [Header("Screen Shake Settings")]
    [SerializeField] private float shakeDuration = 0.15f;
    [SerializeField] private float shakeMagnitude = 0.2f;

    private bool hasHit = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        if (other.CompareTag("Enemy"))
        {
            hasHit = true;

            if (hitVfx != null)
            {
                GameObject vfx = Instantiate(hitVfx, transform.position, Quaternion.identity);
                Destroy(vfx, 0.5f);
            }

            // หา EnemyDeath จากตัวที่ชน หรือ parent ของมัน
            EnemyDeath enemyDeath = other.GetComponent<EnemyDeath>();

            if (enemyDeath == null)
                enemyDeath = other.GetComponentInParent<EnemyDeath>();

            if (enemyDeath != null)
            {
                enemyDeath.Die();
            }
            else
            {
                Debug.LogWarning("Enemy ไม่มี EnemyDeath ให้ใส่สคริปต์ EnemyDeath ที่ตัว Enemy หลัก");
            }

            StartCoroutine(ShakeCameraAndDestroy());
        }
    }

    private IEnumerator ShakeCameraAndDestroy()
    {
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
            sr.enabled = false;

        if (TryGetComponent<Collider2D>(out Collider2D col))
            col.enabled = false;

        if (Camera.main != null)
        {
            Transform cameraTransform = Camera.main.transform;
            Vector3 originalPos = cameraTransform.localPosition;
            float elapsed = 0f;

            while (elapsed < shakeDuration)
            {
                float x = Random.Range(-1f, 1f) * shakeMagnitude;
                float y = Random.Range(-1f, 1f) * shakeMagnitude;

                cameraTransform.localPosition = new Vector3(
                    originalPos.x + x,
                    originalPos.y + y,
                    originalPos.z
                );

                elapsed += Time.deltaTime;
                yield return null;
            }

            cameraTransform.localPosition = originalPos;
        }

        Destroy(gameObject);
    }
}