using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;       // Префаб пули
    public Transform firePoint;           // Точка, откуда появляется пуля
    public float shootCooldown = 0.5f;      // Задержка между выстрелами
    public float projectileSpeed = 15f;     // Скорость пули

    [Header("Audio Settings")]
    public AudioClip shootSound;          // Звук выстрела
    [SerializeField] private float shootVolume = 1f; // Громкость звука (настраиваемое через инспектор)

    private float shootTimer = 0f;
    private Animator animator;
    private PlayerStats stats;

    void Start()
    {
        animator = GetComponent<Animator>();
        stats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        shootTimer += Time.deltaTime;

        // Стрельба по нажатию клавиши F
        if (Input.GetKeyDown(KeyCode.F) && shootTimer >= shootCooldown)
        {
            if (stats != null && stats.UseAmmo()) // стреляем только если патроны есть
            {
                animator.SetInteger("State", 3);
                Shoot();
                shootTimer = 0f;
            }
            else
            {
                Debug.Log("Нет патронов!");
            }
        }
    }

    void Shoot()
    {
        // Запускаем анимацию стрельбы
        if (animator != null)
            animator.SetTrigger("Shoot");

        // Создаем пулю у firePoint
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Определяем горизонтальное направление выстрела:
        // Если scale.x положительный, игрок смотрит вправо => стрелять вправо; иначе — влево.
        int direction = transform.localScale.x > 0 ? 1 : -1;

        // Если у пули есть скрипт Bullet с методом SetDirection, вызываем его.
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction, projectileSpeed);
        }
        else
        {
            // Если скрипта нет, устанавливаем скорость через Rigidbody2D.
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Отключаем гравитацию, если нужно, и замораживаем вращение
                rb.gravityScale = 0f;
                rb.freezeRotation = true;
                rb.linearVelocity = new Vector2(direction, 0f) * projectileSpeed;
            }
        }

        // Воспроизведение звука выстрела через временный AudioSource
        PlayShootSound();
    }

    void PlayShootSound()
    {
        if (shootSound == null)
            return;

        GameObject tempGO = new GameObject("TempShootAudio");
        tempGO.transform.position = transform.position;

        AudioSource tempSource = tempGO.AddComponent<AudioSource>();
        tempSource.clip = shootSound;
        tempSource.volume = shootVolume;
        tempSource.Play();

        Destroy(tempGO, shootSound.length);
    }
}
