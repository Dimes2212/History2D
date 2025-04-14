using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public int ammoAmount = 5;  // Количество патронов, которое даст этот предмет

    [Header("Audio Settings")]
    public AudioSource audioSource;    // Источник звука (опционально)
    public AudioClip pickupSound;      // Звук подбирания
    [SerializeField] private float pickupVolume = 1f;

    void Start()
    {
        // Если источник звука не назначен, можно добавить его автоматически
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Если предмет подбирает игрок (тег "Player")
        if (other.CompareTag("Player"))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.AddAmmo(ammoAmount); // Добавляем патроны игроку

                // Воспроизводим звук подбирания, если задан
                if (audioSource != null && pickupSound != null)
                {
                    audioSource.volume = pickupVolume;
                    audioSource.PlayOneShot(pickupSound);
                }
            }
            Destroy(gameObject);
        }
    }
}
