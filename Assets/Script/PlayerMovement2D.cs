using UnityEngine;

public class PlayerCurveMovement : MonoBehaviour
{
    public float steerAmount = 3f;
    public float steerSpeed = 5f;
    public float returnSpeed = 2f;
    public float smoothTime = 0.15f;
    public float returnYPower = 5f;

    public float minX = -5f;
    public float maxX = 5f;

    [Header("Rotation")]
    public float rotationAmount = 25f;
    public float rotationSmooth = 10f;

    private float targetX;
    private float currentVelocity;
    private float startY;
    private float input;

    [HideInInspector] public bool isKnockback = false;

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        if (isKnockback) return;

        input = Input.GetAxisRaw("Horizontal");

        if (input != 0)
        {
            targetX += input * steerSpeed * Time.deltaTime;
            targetX = Mathf.Clamp(targetX, -steerAmount, steerAmount);
        }
        else
        {
            targetX = Mathf.Lerp(targetX, 0f, returnSpeed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        if (isKnockback) return;

        Vector3 pos = transform.position;

        pos.y = Mathf.Lerp(pos.y, startY, Time.fixedDeltaTime * returnYPower);

        float newX = Mathf.SmoothDamp(
            pos.x,
            targetX,
            ref currentVelocity,
            smoothTime
        );

        pos.x = Mathf.Clamp(newX, minX, maxX);
        transform.position = pos;

        // เอียงตามปุ่มซ้ายขวา
        float targetRotationZ = -input * rotationAmount;

        // ถ้าไม่ได้กด ให้กลับมาตรง
        if (input == 0)
        {
            targetRotationZ = 0f;
        }

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0f, 0f, targetRotationZ),
            Time.fixedDeltaTime * rotationSmooth
        );
    }
}