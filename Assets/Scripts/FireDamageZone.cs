using UnityEngine;

public class FireDamageZone : MonoBehaviour
{
    public int damagePerTick = 1;            // Сколько урона наносится за тик
    public float damageInterval = 1f;        // Интервал между ударами огня (в секундах)
    public AudioClip fireSound;              // Звук при нанесении урона
    public float fireVolume = 1f;

    private bool playerInZone = false;
    private float timer = 0f;
    private AudioSource audioSource;

    void Start()
    {
        if (fireSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = fireSound;
            audioSource.volume = fireVolume;
            audioSource.loop = false;
        }
    }

    void Update()
    {
        if (playerInZone)
        {
            timer += Time.deltaTime;

            if (timer >= damageInterval)
            {
                timer = 0f;
                DealDamage();
            }
        }
    }

    void DealDamage()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerStats stats = player.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage(damagePerTick);

                if (fireSound != null && audioSource != null)
                    audioSource.Play();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            timer = damageInterval; // наносим урон сразу при входе
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            timer = 0f;
        }
    }
}
