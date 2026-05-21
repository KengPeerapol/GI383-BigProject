using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections; // อย่าลืมบรรทัดนี้สำหรับ Coroutine

public class LevelMenu : MonoBehaviour
{
    public Button[] buttons;

    private void Awake()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

        for (int i = 0; i < unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }

    public void OpenLevel(int levelId)
    {
        // สั่งให้เริ่มการหน่วงเวลาก่อนโหลดฉาก
        StartCoroutine(LoadLevelWithDelay(levelId));
    }

    IEnumerator LoadLevelWithDelay(int levelId)
    {
        // รอเวลา 0.3 วินาที เพื่อให้เสียง UI เล่นจนจบก่อน
        yield return new WaitForSecondsRealtime(0.3f);

        string levelName = "Level " + levelId;
        Time.timeScale = 1;
        SceneManager.LoadScene(levelName);
    }
}