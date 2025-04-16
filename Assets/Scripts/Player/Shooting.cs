//using System.Collections;
//using UnityEngine;

//public class PlayerShooting : MonoBehaviour
//{
//    [Header("Shooting Settings")]
//    public GameObject bulletPrefab;
//    public Transform firePoint;
//    public float shootCooldown = 0.5f;
//    public float projectileSpeed = 30f;

//    [Header("Melee Settings")]
//    public float meleeRange = 0.1f;        // Маленький радиус атаки
//    public int meleeDamage = 1;            // Урон ближней атакой
//    public float meleeCooldown = 1f;       // Задержка между атаками
//    public Transform meleePoint;           // Точка атаки
//    public LayerMask enemyLayer;           // Слой врагов

//    [Header("Audio Settings")]
//    public AudioClip shootSound;           // Звук выстрела
//    public AudioClip meleeSound;           // Звук атаки
//    [SerializeField] private float shootVolume = 1f;
//    [SerializeField] private float meleeVolume = 1f;

//    private float shootTimer = 0f;
//    private float meleeTimer = 0f;

//    private Animator animator;
//    private PlayerStats stats;

//    void Start()
//    {
//        animator = GetComponent<Animator>();
//        stats = GetComponent<PlayerStats>();
//    }

//    void Update()
//    {
//        shootTimer += Time.deltaTime;
//        meleeTimer += Time.deltaTime;

//        // Обработка выстрела
//        if (Input.GetKeyDown(KeyCode.F) && shootTimer >= shootCooldown)
//        {
//            if (stats != null && stats.UseAmmo())
//            {
//                animator.SetInteger("State", 3);
//                Shoot();
//                shootTimer = 0f;
//            }
//            else
//            {
//                Debug.Log("Нет патронов!");
//            }
//        }

//        // Обработка ближней атаки
//        if (Input.GetKeyDown(KeyCode.E) && meleeTimer >= meleeCooldown)
//        {
//            animator.SetTrigger("Melee");
//            MeleeAttack();
//            meleeTimer = 0f;
//        }
//    }

//    void Shoot()
//    {
//        if (animator != null)
//            animator.SetTrigger("Shoot");

//        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

//        int direction = transform.localScale.x > 0 ? 1 : -1;

//        Bullet bulletScript = bullet.GetComponent<Bullet>();
//        if (bulletScript != null)
//        {
//            bulletScript.SetDirection(direction, projectileSpeed);
//        }
//        else
//        {
//            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
//            if (rb != null)
//            {
//                rb.gravityScale = 0f;
//                rb.freezeRotation = true;
//                rb.linearVelocity = new Vector2(direction, 0f) * projectileSpeed;
//            }
//        }

//        PlaySound(shootSound, shootVolume);
//    }

//    // Метод для выполнения ближней атаки
//    void MeleeAttack()
//    {
//        // Проверка, какие враги попадают в радиус ближней атаки
//        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(meleePoint.position, meleeRange, enemyLayer);

//        // Отладочные сообщения
//        Debug.Log($"Мelee attack: Радиус {meleeRange} | Точка атаки {meleePoint.position}");

//        // Проходим по всем врагам, которые были пойманы в радиусе атаки
//        foreach (Collider2D enemy in hitEnemies)
//        {
//            // Если это враг, наносим ему урон
//            EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
//            EnemyDamage enemyDamage = enemy.GetComponent<EnemyDamage>();

//            if (enemyStats != null)
//            {
//                // Наносим урон врагу
//                enemyStats.TakeDamage(meleeDamage);

//                // Если у врага есть визуальный эффект, то применяем его
//                if (enemyDamage != null)
//                {
//                    StartCoroutine(enemyDamage.FlashDamageColor());
//                }

//                // Отладочная информация
//                Debug.Log($"Враг поразился: {enemy.gameObject.name}");
//            }
//        }

//        // Воспроизводим звук атаки
//        PlaySound(meleeSound, meleeVolume);
//    }

//    // Метод для воспроизведения звуков
//    void PlaySound(AudioClip clip, float volume)
//    {
//        if (clip == null) return;

//        GameObject tempGO = new GameObject("TempAudio");
//        tempGO.transform.position = transform.position;

//        AudioSource tempSource = tempGO.AddComponent<AudioSource>();
//        tempSource.clip = clip;
//        tempSource.volume = volume;
//        tempSource.Play();

//        Destroy(tempGO, clip.length);
//    }

//    // Отображение радиуса атаки в редакторе (для удобства)
//    void OnDrawGizmosSelected()
//    {
//        if (meleePoint == null) return;

//        Gizmos.color = Color.red;
//        Gizmos.DrawWireSphere(meleePoint.position, meleeRange); // Показываем радиус атаки в редакторе
//    }
//}


using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootCooldown = 0.5f;
    public float projectileSpeed = 30f;

    [Header("Melee Settings")]
    public float meleeRange = 1f;        // Радиус атаки
    public int meleeDamage = 1;          // Урон ближней атакой
    public float meleeCooldown = 1f;     // Задержка между атаками
    public Transform meleePoint;         // Точка атаки
    public LayerMask enemyLayer;         // Слой врагов

    [Header("Audio Settings")]
    public AudioClip shootSound;         // Звук выстрела
    public AudioClip meleeSound;         // Звук атаки
    [SerializeField] private float shootVolume = 1f;
    [SerializeField] private float meleeVolume = 1f;

    private float shootTimer = 0f;
    private float meleeTimer = 0f;

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
        meleeTimer += Time.deltaTime;

        // Обработка выстрела
        if (Input.GetKeyDown(KeyCode.F) && shootTimer >= shootCooldown)
        {
            if (stats != null && stats.UseAmmo())
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

        // Обработка ближней атаки
        if (Input.GetKeyDown(KeyCode.E) && meleeTimer >= meleeCooldown)
        {
            animator.SetTrigger("Melee");
            MeleeAttack();
            meleeTimer = 0f;
        }
    }

    void Shoot()
    {
        if (animator != null)
            animator.SetTrigger("Shoot");

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        int direction = transform.localScale.x > 0 ? 1 : -1;

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDirection(direction, projectileSpeed);
        }
        else
        {
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0f;
                rb.freezeRotation = true;
                rb.linearVelocity = new Vector2(direction, 0f) * projectileSpeed;
            }
        }

        PlaySound(shootSound, shootVolume);
    }

    // Метод для выполнения ближней атаки
    void MeleeAttack()
    {
        // Используем капсульный коллайдер врага для проверки попадания
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(meleePoint.position, meleeRange, enemyLayer);

        // Отладочные сообщения
        Debug.Log($"Мelee attack: Проверка попадания с радиусом {meleeRange}.");

        // Проверяем попадания врагов
        foreach (Collider2D enemy in hitEnemies)
        {
            // Получаем капсульный коллайдер врага
            CapsuleCollider2D enemyCollider = enemy.GetComponent<CapsuleCollider2D>();

            if (enemyCollider != null)
            {
                // Создаем временный коллайдер для зоны атаки игрока
                Collider2D attackCollider = meleePoint.GetComponent<Collider2D>();

                // Проверяем, пересекается ли коллайдер зоны атаки с капсульным коллайдером врага
                if (enemyCollider.IsTouching(attackCollider))
                {
                    // Если пересекаются, наносим урон
                    EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
                    EnemyDamage enemyDamage = enemy.GetComponent<EnemyDamage>();

                    if (enemyStats != null)
                    {
                        enemyStats.TakeDamage(meleeDamage);
                    }

                    if (enemyDamage != null)
                    {
                        StartCoroutine(enemyDamage.FlashDamageColor());
                    }

                    // Отладочное сообщение
                    Debug.Log($"Враг поразился: {enemy.gameObject.name}");
                }
            }
        }

        // Воспроизведение звука атаки
        PlaySound(meleeSound, meleeVolume);
    }

    // Метод для воспроизведения звуков
    void PlaySound(AudioClip clip, float volume)
    {
        if (clip == null) return;

        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = transform.position;

        AudioSource tempSource = tempGO.AddComponent<AudioSource>();
        tempSource.clip = clip;
        tempSource.volume = volume;
        tempSource.Play();

        Destroy(tempGO, clip.length);
    }

    // Отображение радиуса атаки в редакторе
    void OnDrawGizmosSelected()
    {
        if (meleePoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(meleePoint.position, meleeRange); // Показываем радиус атаки
    }
}
