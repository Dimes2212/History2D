using UnityEngine;

public class ApplePickup : MonoBehaviour
{
    public AudioClip eatSound;         // Звук съедения яблока
    public float volume = 1f;          // Громкость

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Только если вошёл игрок
        {
            PlayEatSound();
            Destroy(gameObject);       // Удаляем яблоко
        }
    }

    void PlayEatSound()
    {
        if (eatSound == null) return;

        GameObject tempGO = new GameObject("TempAppleSound");
        tempGO.transform.position = transform.position;

        AudioSource audioSource = tempGO.AddComponent<AudioSource>();
        audioSource.clip = eatSound;
        audioSource.volume = volume;
        audioSource.Play();

        Destroy(tempGO, eatSound.length);
    }
}
