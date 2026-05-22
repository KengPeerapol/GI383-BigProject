using UnityEngine;

public class PlayerCurveMovement : MonoBehaviour
{
    //public float forwardSpeed = 8f;

    public float steerAmount = 3f;       // ระยะออกสูงสุด
    public float steerSpeed = 5f;        // ความไวตอนกด
    public float returnSpeed = 2f;       // ความเร็วกลับกลาง
    public float smoothTime = 0.15f;     // ความเนียนของโค้ง
    public float returnYPower = 5f;

    public float minX = -5f;
    public float maxX = 5f;

    private float targetX;
    private float currentVelocity;
    private float startY;
    

    [SerializeField]
    private float rotationSpeed;
    [HideInInspector] public bool isKnockback = false;

    void Start()
    {
        startY = transform.position.y;
    }
    
    void Update()
    {
        if (isKnockback) return;

        float input = Input.GetAxisRaw("Horizontal");
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
        float oldX = pos.x;
        
        pos.y = Mathf.Lerp(pos.y, startY, Time.fixedDeltaTime * returnYPower);

        float newX = Mathf.SmoothDamp(
            pos.x,
            targetX,
            ref currentVelocity,
            smoothTime
        );

        pos.x = Mathf.Clamp(newX, minX, maxX);
        transform.position = pos;
        
        float deltaX = newX - oldX;
        float targetRotation = -deltaX * rotationSpeed;
        
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0f, 0f, targetRotation),
            Time.fixedDeltaTime * 10f);
        
    }
}