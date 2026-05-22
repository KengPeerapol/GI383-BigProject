using UnityEngine;
using UnityEngine.UI;

public class Distance : MonoBehaviour
{
    [SerializeField] public Image finishLine;
    [SerializeField] public Player player;
    [SerializeField] public CameraMove camera;
    [SerializeField] public MoveDown moveDown;
    [SerializeField] public PlayerCurveMovement playerCurve;

    public int distance = 0;
    
    // Update is called once per frame
    void Update()
    {
        distance = (int)(finishLine.transform.position.y - player.transform.position.y);
        if (distance <= 8)
        {
            camera.enabled = false;
            playerCurve.enabled = false;
            moveDown.enabled = true;
        }
    }
}
