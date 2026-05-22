using UnityEngine;

public class MoveDown : MonoBehaviour
{

    public float speed = 10f;
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += Vector3.up * speed * Time.fixedDeltaTime;
    }
}
