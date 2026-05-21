using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // ต้องเพิ่มบรรทัดนี้เพื่อใช้ Coroutine (หน่วงเวลา)

public class GoToMenu : MonoBehaviour
{
    public string menuSceneName = "Main Menu";

    public void BackToMenu()
    {
        // เรียกใช้ฟังก์ชันหน่วงเวลา
        StartCoroutine(MenuWithDelay());
    }

    IEnumerator MenuWithDelay()
    {
        // รอเวลา 0.3 วินาที (เพื่อให้เสียง UI เล่นจนจบ)
        yield return new WaitForSecondsRealtime(0.3f);

        // คืนค่าเวลาเป็นปกติ และโหลดหน้า Menu
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }
}