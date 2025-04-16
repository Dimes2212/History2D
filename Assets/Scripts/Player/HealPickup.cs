using UnityEngine;

public class HealPickup : MonoBehaviour
{
    public int healAmount = 25;                // Сколько здоровья восстанавливает
    public AudioClip healSound;                // Звук при подборе
    public float volume = 1f;                  // Громкость

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, игрок ли это
        PlayerStats player = other.GetComponent<PlayerStats>();
        if (player != null)
        {
            player.Heal(healAmount);
            PlaySound();
            Destroy(gameObject); // Удаляем аптечку после использования
        }
    }

    void PlaySound()
    {
        if (healSound == null) return;

        GameObject tempGO = new GameObject("TempHealSound");
        tempGO.transform.position = transform.position;

        AudioSource audioSource = tempGO.AddComponent<AudioSource>();
        audioSource.clip = healSound;
        audioSource.volume = volume;
        audioSource.Play();

        Destroy(tempGO, healSound.length);
    }
}
