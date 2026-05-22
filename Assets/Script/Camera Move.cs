using UnityEngine;

public class CameraMove : MonoBehaviour
{

    public float forwardSpeed = -8f;        // ��觢�鹵�ʹ 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        // ��觢�鹵�ʹ 
        transform.position += Vector3.up * forwardSpeed * Time.fixedDeltaTime;
    }
}
