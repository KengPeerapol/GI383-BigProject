using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GoToMenu : MonoBehaviour
{
    public string menuSceneName = "Main Menu";

    public void BackToMenu()
    {
        StartCoroutine(MenuWithDelay());
    }

    IEnumerator MenuWithDelay()
    {
        yield return new WaitForSecondsRealtime(0.3f);

        // คืนค่าก่อนกลับเมนู
        Time.timeScale = 1f;
        CameraMove.StartAllMove();

        SceneManager.LoadScene(menuSceneName);
    }
}