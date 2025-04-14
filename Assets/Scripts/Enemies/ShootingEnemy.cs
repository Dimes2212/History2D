using UnityEngine;

[RequireComponent(typeof(EnemyControl))]
public class ShootingEnemy : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private Vector2 detectionAreaSize = new Vector2(8f, 4f);
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [Header("Detection Settings")]
    [SerializeField] private LayerMask obstacleLayer;

    [Header("Audio")] // 🎵 Новый блок
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private float shootSoundVolume = 1f; // Уровень громкости звука выстрела

    private Transform player;
    private EnemyControl enemyControl;
    private float nextFireTime;
    private bool playerDetected;

    void Start()
    {
        enemyControl = GetComponent<EnemyControl>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        InitializeDetectionArea();
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
        Vector2 playerPosition = player.position;
        Vector2 enemyPosition = transform.position;

        return Mathf.Abs(playerPosition.x - enemyPosition.x) <= detectionAreaSize.x / 2 &&
               Mathf.Abs(playerPosition.y - enemyPosition.y) <= detectionAreaSize.y / 2;
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
        float xDirection = player.position.x - transform.position.x;
        bool shouldFaceRight = xDirection > 0;

        if (shouldFaceRight != enemyControl.IsFacingRight)
        {
            enemyControl.Flip();
        }
    }

    void Shoot()
    {
        if (projectilePrefab && firePoint)
        {
            Vector2 fireDirection = enemyControl.IsFacingRight ? Vector2.right : Vector2.left;

            GameObject projectile = Instantiate(
                projectilePrefab,
                firePoint.position,
                Quaternion.identity
            );

            projectile.GetComponent<Rigidbody2D>().linearVelocity = fireDirection * 10f;
            Destroy(projectile, 2f);

            // 🔊 Воспроизведение звука выстрела
            if (audioSource && shootSound)
            {
                audioSource.volume = shootSoundVolume;  // Устанавливаем громкость звука
                audioSource.PlayOneShot(shootSound);
            }
        }
    }

    bool HasLineOfSight()
    {
        Vector2 rayDirection = (player.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            rayDirection,
            detectionAreaSize.magnitude,
            obstacleLayer
        );
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

        if (enemyControl != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 direction = enemyControl.IsFacingRight ? Vector3.right : Vector3.left;
            Gizmos.DrawRay(firePoint.position, direction * 5f);
        }
    }
}
