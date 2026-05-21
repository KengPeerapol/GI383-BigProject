using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("----------- Audio Source -----------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("----------- Audio Clip -----------")]
    public AudioClip background;
    // --- เสียงที่เพิ่มเข้ามาใหม่ ---
    public AudioClip uiClick;
    public AudioClip playerShoot;
    public AudioClip ammoChargeFull;
    public AudioClip playerHit;
    public AudioClip victory;
    public AudioClip death; // (ใช้เป็นเสียงตอนแพ้/Game Over ด้วยได้เลย)

    private void Start()
    {
        if (background != null)
        {
            musicSource.clip = background;
            musicSource.Play();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}