using UnityEngine;

public class PlayerCurveMovement : MonoBehaviour
{
    //public float forwardSpeed = 8f;

    public float steerAmount = 3f;       // ระยะออกสูงสุด
    public float steerSpeed = 5f;        // ความไวตอนกด
    public float returnSpeed = 2f;       // ความเร็วกลับกลาง
    public float smoothTime = 0.15f;     // ความเนียนของโค้ง

    public float minX = -5f;
    public float maxX = 5f;

    private float targetX;
    private float currentVelocity;

    void Update()
    {
        float input = Input.GetAxisRaw("Horizontal");

        if (input != 0)
        {
            // ตอนกด → เปลี่ยน target
            targetX += input * steerSpeed * Time.deltaTime;
            targetX = Mathf.Clamp(targetX, -steerAmount, steerAmount);
        }
        else
        {
            // ตอนปล่อย → target ค่อย ๆ กลับ 0
            targetX = Mathf.Lerp(targetX, 0f, returnSpeed * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {
        Vector3 pos = transform.position;

        // วิ่งขึ้นตลอด
       // pos.y += forwardSpeed * Time.fixedDeltaTime;

        // ดึง X แบบโค้งเนียน
        float newX = Mathf.SmoothDamp(
            pos.x,
            targetX,
            ref currentVelocity,
            smoothTime
        );

        pos.x = Mathf.Clamp(newX, minX, maxX);

        transform.position = pos;
    }
}