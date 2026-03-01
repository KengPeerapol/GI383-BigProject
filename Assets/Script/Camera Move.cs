using UnityEngine;

public class CameraMove : MonoBehaviour
{

    public float forwardSpeed = -8f;        // วิ่งขึ้นตลอด

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        // วิ่งขึ้นตลอด
        transform.position += Vector3.up * forwardSpeed * Time.fixedDeltaTime;
    }
}
