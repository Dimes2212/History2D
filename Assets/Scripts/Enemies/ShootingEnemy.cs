using UnityEngine;

[RequireComponent(typeof(EnemyControl))]
public class ShootingEnemy : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private Vector2 detectionAreaSize = new Vector2(8f, 4f);
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint; // Точка, где появляется пуля

    [Header("Detection Settings")]
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private float shootSoundVolume = 1f;

    [Header("Projectile Settings")]
    [SerializeField] private float projectileSpeed = 10f;

    private Transform player;
    private EnemyControl enemyControl;
    private float nextFireTime;
    private bool playerDetected;
    private Animator animator;

    void Start()
    {
        enemyControl = GetComponent<EnemyControl>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InitializeDetectionArea();
        animator = GetComponent<Animator>();
    }

    void InitializeDetectionArea()
    {
        BoxCollider2D detector = gameObject.AddComponent<BoxCollider2D>();
        detector.isTrigger = true;
        detector.size = detectionAreaSize;
    }

    void Update()
    {
        if (playerDetected && IsPlayerInCombatArea())
        {
            HandleCombat();
        }
        else
        {
            enemyControl.ResumePatrol();
        }
    }

    bool IsPlayerInCombatArea()
    {
        Vector2 playerPos = player.position;
        Vector2 enemyPos = transform.position;
        return Mathf.Abs(playerPos.x - enemyPos.x) <= detectionAreaSize.x / 2 &&
               Mathf.Abs(playerPos.y - enemyPos.y) <= detectionAreaSize.y / 2;
    }

    void HandleCombat()
    {
        enemyControl.StopPatrol();
        FacePlayer();
        if (Time.time >= nextFireTime && HasLineOfSight())
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void FacePlayer()
    {
        float xDiff = player.position.x - transform.position.x;
        bool shouldFaceRight = xDiff > 0;

        if (shouldFaceRight != enemyControl.IsFacingRight)
        {
            enemyControl.Flip();
        }
    }

    void Shoot()
    {
        if (projectilePrefab && firePoint)
        {
            animator.SetInteger("EnemyState", 2);

            // Определяем направление выстрела в зависимости от позиции игрока
            float horizontalDir = (player.position.x - transform.position.x) > 0 ? 1f : -1f;
            Vector2 fireDirection = new Vector2(horizontalDir, 0f); // только по горизонтали

            // Создаем пулю в позиции firePoint
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0f;
                rb.freezeRotation = true;
                rb.linearVelocity = fireDirection * projectileSpeed; // корректная установка скорости
            }

            // Игнорируем столкновение пули с врагом
            Collider2D projCollider = projectile.GetComponent<Collider2D>();
            Collider2D enemyCollider = GetComponent<Collider2D>();
            if (projCollider != null && enemyCollider != null)
            {
                Physics2D.IgnoreCollision(projCollider, enemyCollider);
            }

            Destroy(projectile, 2f);

            // Проигрываем звук выстрела
            if (audioSource && shootSound)
            {
                audioSource.volume = shootSoundVolume;
                audioSource.PlayOneShot(shootSound);
            }
        }
    }

    bool HasLineOfSight()
    {
        Vector2 rayDir = (player.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDir, detectionAreaSize.magnitude, obstacleLayer);
        return hit.collider == null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetected = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawWireCube(transform.position, detectionAreaSize);
        if (firePoint != null)
        {
            Gizmos.color = Color.yellow;
            // Рисуем строго горизонтальную линию из firePoint в направлении, определенном по оси X
            float horizontalDir = (player != null && player.position.x - transform.position.x >= 0) ? 1f : -1f;
            Vector3 drawDir = new Vector3(horizontalDir, 0f, 0f);
            Gizmos.DrawRay(firePoint.position, drawDir * 5f);
        }
    }
}
