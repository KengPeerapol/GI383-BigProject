using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public GameObject container;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenpauseMenu();
        }
    }

    public void OpenpauseMenu()
    {
        container.SetActive(true);
        Time.timeScale = 0;

        // --- เพิ่ม: สั่งพักเพลง Background ---
        GameObject audioObj = GameObject.FindGameObjectWithTag("Sound");
        if (audioObj != null)
        {
            SoundManager audioManager = audioObj.GetComponent<SoundManager>();
            if (audioManager != null)
            {
                audioManager.PauseMusic();
            }
        }
    }

    public void ResumeButton()
    {
        // 1. ฝากให้ SoundManager เล่นเสียง UI ให้
        GameObject audioObj = GameObject.FindGameObjectWithTag("Sound");
        if (audioObj != null)
        {
            SoundManager audioManager = audioObj.GetComponent<SoundManager>();
            if (audioManager != null)
            {
                if (audioManager.uiClick != null)
                {
                    audioManager.PlaySFX(audioManager.uiClick);
                }

                // --- เพิ่ม: สั่งให้เพลง Background กลับมาเล่นต่อ ---
                audioManager.ResumeMusic();
            }
        }

        // 2. ปิดหน้าต่างและให้เวลาเดินปกติ
        container.SetActive(false);
        Time.timeScale = 1;
    }

    public void MainMenuButton()
    {
        StartCoroutine(LoadMainMenuWithDelay());
    }

    IEnumerator LoadMainMenuWithDelay()
    {
        yield return new WaitForSecondsRealtime(0.3f);
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }
}