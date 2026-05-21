using UnityEngine;

public class Ammo : MonoBehaviour
{
    public float currentAmmo;
    public float maxAmmo = 1;

    // เพิ่มตัวแปรไว้เช็กว่าเล่นเสียงเต็มไปหรือยัง (ป้องกันเสียงเล่นรัวๆ)
    private bool isFullSoundPlayed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentAmmo = maxAmmo;
        isFullSoundPlayed = true; // ตอนเริ่มเกมกระสุนเต็มอยู่แล้ว ไม่ต้องเล่นเสียง
    }

    // Update is called once per frame
    void Update()
    {
        // แก้ไขจากการเช็ก == 1 เป็นการเช็ก >= maxAmmo ป้องกันบัคทศนิยมไม่เป๊ะ
        if (currentAmmo >= maxAmmo)
        {
            currentAmmo = maxAmmo; // ล็อกค่ากระสุนไม่ให้เกิน

            // ถ้ากระสุนเต็ม และยังไม่ได้เล่นเสียงเตือน
            if (!isFullSoundPlayed)
            {
                PlayAmmoFullSound();
                isFullSoundPlayed = true; // เปลี่ยนสถานะว่าเล่นไปแล้ว
            }
        }
        else
        {
            // ถ้ากระสุนถูกยิงออกไป (ค่าน้อยกว่า max) ให้เตรียมพร้อมรอเล่นเสียงรอบหน้า
            isFullSoundPlayed = false;
        }
    }

    // สร้างฟังก์ชันแยกสำหรับเรียกเสียง จะได้ดูเป็นระเบียบ
    void PlayAmmoFullSound()
    {
        GameObject audioObj = GameObject.FindGameObjectWithTag("Sound");
        if (audioObj != null)
        {
            SoundManager audioManager = audioObj.GetComponent<SoundManager>();
            if (audioManager != null && audioManager.ammoChargeFull != null)
            {
                audioManager.PlaySFX(audioManager.ammoChargeFull);
            }
        }
    }
}