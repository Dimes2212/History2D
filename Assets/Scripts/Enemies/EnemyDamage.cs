using System.Collections;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public EnemyStats stats;              // Ссылка на скрипт характеристик врага
    public SpriteRenderer spriteRenderer;  // Рендерер врага для визуальных эффектов
    public Color damageColor = Color.red;  // Цвет при получении урона
    public float colorDuration = 0.2f;     // Время мигания

    [Header("Audio")]
    public AudioSource audioSource;       // Источник звука
    public AudioClip deathSound;          // Звук смерти
    [SerializeField] private float deathSoundVolume = 1f; // Громкость звука смерти
    [SerializeField] private float deathDelay = 0.5f;      // Задержка перед уничтожением, чтобы звук успел проиграться

    [Header("Pickup Settings")]
    public GameObject ammoPickupPrefab;   // Префаб предмета-подбиралки, который даёт патроны

    private Color originalColor;

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        originalColor = spriteRenderer.color;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Если столкновение произошло с пулей
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            Destroy(collision.gameObject);   // Уничтожаем пулю

            // Наносим урон врагу
            stats.TakeDamage(1);

            // Запускаем визуальный эффект (мигание)
            StartCoroutine(FlashDamageColor());

            // Если здоровье равно 0, инициируем смерть врага
            if (stats.currentHealth <= 0)
            {
                Debug.Log("Здоровье врага 0, вызываем Die()");
                StartCoroutine(DieWithDelay());
            }
        }
    }

    IEnumerator FlashDamageColor()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(colorDuration);
        spriteRenderer.color = originalColor;
    }

    IEnumerator DieWithDelay()
    {
        // Воспроизведение звука смерти
        if (audioSource != null && deathSound != null)
        {
            Debug.Log("Воспроизведение звука смерти");
            audioSource.volume = deathSoundVolume;
            audioSource.PlayOneShot(deathSound);
        }
        else
        {
            Debug.LogWarning("AudioSource или deathSound не назначены!");
        }

        // Создаем предмет-подбиралку для патронов
        if (ammoPickupPrefab != null)
        {
            Instantiate(ammoPickupPrefab, transform.position, Quaternion.identity);
        }

        // Задержка перед уничтожением врага
        yield return new WaitForSeconds(deathDelay);

        Destroy(gameObject); // Уничтожаем врага
    }
}
