using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float forwardSpeed = -8f;

    public static bool canMove = true;

    void OnEnable()
    {
        // เวลา Object ถูกสร้างใหม่ในฉาก ให้กลับมาขยับได้
        canMove = true;
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        transform.position += Vector3.up * forwardSpeed * Time.fixedDeltaTime;
    }

    public static void StopAllMove()
    {
        canMove = false;
    }

    public static void StartAllMove()
    {
        canMove = true;
    }
}