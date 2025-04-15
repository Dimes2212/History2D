using UnityEngine;

[RequireComponent(typeof(EnemyControl))]
[RequireComponent(typeof(Rigidbody2D))]
public class MeleeEnemy : MonoBehaviour
{
    [Header("Combat Settings")]
    [SerializeField] private Vector2 detectionAreaSize = new Vector2(3f, 2f); // Размер зоны обнаружения
    [SerializeField] private float moveSpeed = 2f; // Скорость перемещения
    [SerializeField] private float attackRate = 1f; // Скорость атаки
    [SerializeField] private float attackRange = 1.5f; // Расстояние для атаки

    [Header("Detection Settings")]
    [SerializeField] private LayerMask playerLayer; // Для поиска игрока
    [SerializeField] private LayerMask obstacleLayer; // Для поиска препятствий

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private float attackSoundVolume = 1f;

    [Header("Melee Settings")]
    [SerializeField] private EnemySword sword; // Меч врага

    private Transform player;
    private Rigidbody2D rb;
    private EnemyControl enemyControl;
    private float nextAttackTime;
    private bool playerDetected;
    private bool swordIsEnabled = false;
    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            if (sword != null && swordIsEnabled)
            {
                sword.DisableDamage();
                swordIsEnabled = false;
            }
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

        // Остановка врага на нужном расстоянии
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer > attackRange)
        {
            MoveTowardPlayer();
        }
        else
        {
            // Останавливаем врага, если он близко к игроку
            rb.linearVelocity = Vector2.zero;

            // Если враг находится в зоне атаки, начинаем атаку
            if (!isAttacking && Time.time >= nextAttackTime)
            {
                AttackWithSword();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    void MoveTowardPlayer()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y); // Двигаемся только по оси X
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

    void AttackWithSword()
    {
        if (sword != null)
        {
            sword.EnableDamage(); // Включаем урон мечом
            swordIsEnabled = true;
            isAttacking = true;

            // Звук атаки
            if (audioSource && attackSound)
            {
                audioSource.volume = attackSoundVolume;
                audioSource.PlayOneShot(attackSound);
            }

            // После атаки даем время на кулдаун
            Invoke("ResetAttack", 0.5f); // Делаем задержку перед следующей атакой
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
        sword.DisableDamage(); // Отключаем урон мечом после атаки
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
    }
}
