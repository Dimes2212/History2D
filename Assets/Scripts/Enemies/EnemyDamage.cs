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

    // Сделано публичным, чтобы вызывать его в других классах
    public IEnumerator FlashDamageColor()  // изменено на public
    {
        // Проверка на существование spriteRenderer
        if (spriteRenderer == null) yield break;

        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(colorDuration);

        // Проверка перед установкой исходного цвета
        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;
    }

    IEnumerator DieWithDelay()
    {
        // Воспроизведение звука смерти, если источник и звуковой файл существуют
        if (audioSource != null && deathSound != null)
        {
            audioSource.volume = deathSoundVolume;
            audioSource.PlayOneShot(deathSound);
        }

        // Создаем предмет-подбиралку для патронов, если он существует
        if (ammoPickupPrefab != null)
        {
            Instantiate(ammoPickupPrefab, transform.position, Quaternion.identity);
        }

        // Задержка перед уничтожением врага
        yield return new WaitForSeconds(deathDelay);

        // Уничтожаем врага, если объект еще существует
        if (gameObject != null)
            Destroy(gameObject);
    }
}
