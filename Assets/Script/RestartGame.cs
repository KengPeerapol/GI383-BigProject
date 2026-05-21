using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // ต้องเพิ่มบรรทัดนี้เพื่อใช้ Coroutine (หน่วงเวลา)

public class RestartGame : MonoBehaviour
{
    public void Restart()
    {
        // เรียกใช้ฟังก์ชันหน่วงเวลา
        StartCoroutine(RestartWithDelay());
    }

    IEnumerator RestartWithDelay()
    {
        // รอเวลา 0.3 วินาที (เพื่อให้เสียง UI เล่นจนจบ)
        yield return new WaitForSecondsRealtime(0.3f);

        // คืนค่าเวลาเป็นปกติ และโหลดหน้าต่างเดิม
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}