using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scene")]
    public int gameSceneIndex = 1;

    public void PlayGame()
    {
        // คืนเวลาเกมให้ปกติ
        Time.timeScale = 1f;

        // เปิดให้ Object ที่ใช้ CameraMove กลับมาขยับ
        CameraMove.StartAllMove();

        // โหลดฉากเกม
        SceneManager.LoadSceneAsync(gameSceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}