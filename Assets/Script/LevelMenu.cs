using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections; // อย่าลืมบรรทัดนี้สำหรับ Coroutine

public class LevelMenu : MonoBehaviour
{
    public Button[] buttons;

    public void OpenLevel(int levelIndex)
    {
        // สั่งให้เริ่มการหน่วงเวลาก่อนโหลดฉาก
        StartCoroutine(LoadLevelWithDelay(levelIndex));
    }

    IEnumerator LoadLevelWithDelay(int levelIndex)
    {
        // รอเวลา 0.3 วินาที เพื่อให้เสียง UI เล่นจนจบก่อน
        yield return new WaitForSecondsRealtime(0.3f);
        SceneManager.LoadScene(levelIndex);
    }
}