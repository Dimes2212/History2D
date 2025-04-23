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
    public float meleeRange = 1f;
    public int meleeDamage = 1;
    public float meleeCooldown = 1f;
    public Transform meleePoint;
    public LayerMask enemyLayer;

    [Header("Audio Settings")]
    public AudioClip shootSound;
    public AudioClip meleeSound;
    [SerializeField] private float shootVolume = 1f;
    [SerializeField] private float meleeVolume = 1f;

    [Header("Reload Indicator")]
    public GameObject readyIndicatorPrefab;
    public GameObject cooldownIndicatorPrefab;
    public Transform indicatorPoint;

    private GameObject currentIndicator;

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

        if (Input.GetKeyDown(KeyCode.E) && meleeTimer >= meleeCooldown)
        {
            animator.SetTrigger("Melee");
            animator.SetInteger("State", 4);
            MeleeAttack();
            meleeTimer = 0f;
        }

        UpdateShootIndicator();
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

    void MeleeAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(meleePoint.position, meleeRange, enemyLayer);
        Debug.Log($"Melee attack: Проверка попадания с радиусом {meleeRange}.");

        foreach (Collider2D enemy in hitEnemies)
        {
            CapsuleCollider2D enemyCollider = enemy.GetComponent<CapsuleCollider2D>();
            if (enemyCollider != null)
            {
                Collider2D attackCollider = meleePoint.GetComponent<Collider2D>();

                if (attackCollider != null && enemyCollider.IsTouching(attackCollider))
                {
                    EnemyStats enemyStats = enemy.GetComponent<EnemyStats>();
                    EnemyDamage enemyDamage = enemy.GetComponent<EnemyDamage>();

                    if (enemyStats != null)
                        enemyStats.TakeDamage(meleeDamage);

                    if (enemyDamage != null)
                        StartCoroutine(enemyDamage.FlashDamageColor());

                    Debug.Log($"Враг поразился: {enemy.gameObject.name}");
                }
            }
        }

        PlaySound(meleeSound, meleeVolume);
    }

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

    void OnDrawGizmosSelected()
    {
        if (meleePoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(meleePoint.position, meleeRange);
    }

    void UpdateShootIndicator()
    {
        // Удаляем текущий индикатор
        if (currentIndicator != null)
        {
            Destroy(currentIndicator);
        }

        GameObject indicatorPrefab = shootTimer >= shootCooldown ? readyIndicatorPrefab : cooldownIndicatorPrefab;

        if (indicatorPrefab != null && indicatorPoint != null)
        {
            currentIndicator = Instantiate(indicatorPrefab, indicatorPoint.position, Quaternion.identity, indicatorPoint);
        }
    }
}
