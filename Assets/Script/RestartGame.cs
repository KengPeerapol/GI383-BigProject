using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RestartGame : MonoBehaviour
{
    public void Restart()
    {
        StartCoroutine(RestartWithDelay());
    }

    IEnumerator RestartWithDelay()
    {
        // รอแบบไม่สน Time.timeScale
        yield return new WaitForSecondsRealtime(0.3f);

        // คืนค่าทุกอย่างก่อนโหลดฉากใหม่
        Time.timeScale = 1f;
        CameraMove.StartAllMove();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}