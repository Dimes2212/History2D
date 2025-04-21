using UnityEngine;

[RequireComponent(typeof(EnemyControl), typeof(Rigidbody2D))]
public class MeleeEnemy : MonoBehaviour
{
    [Header("Combat Settings")]
    public float detectionRadius = 5f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;
    public int damage = 10;
    public float chaseSpeed = 2f;

    [Header("Animation")]
    public string attackAnimationTrigger = "Attack"; // 👉 слот для триггера

    [Header("Audio")]
    public AudioClip attackSound;
    public AudioSource audioSource;

    private Transform player;
    private EnemyControl enemyControl;
    private Rigidbody2D rb;
    private Animator animator;
    private float lastAttackTime;
    private bool isAttacking;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        enemyControl = GetComponent<EnemyControl>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb.gravityScale = 1;
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRadius)
        {
            enemyControl.StopPatrol();
            FacePlayer();

            if (distance > attackRange)
            {
                MoveTowardsPlayer();
            }
            else
            {
                StopMovement();
                TryAttack();
            }
        }
        else
        {
            StopMovement();
            enemyControl.ResumePatrol();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * chaseSpeed, rb.linearVelocity.y);

        animator?.SetInteger("EnemyState2", 1);
        animator?.SetInteger("State4", 1);// Бег
        animator?.SetInteger("State5", 1);// Бег

    }

    void StopMovement()
    {
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        //animator?.SetInteger("EnemyState2", 0); // Стоит
    }

    void FacePlayer()
    {
        float diff = player.position.x - transform.position.x;
        bool shouldFaceRight = diff > 0;

        if (shouldFaceRight != enemyControl.IsFacingRight)
            enemyControl.Flip();
    }

    void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown || isAttacking) return;
        animator?.SetInteger("EnemyState2", 2); // бьет
        animator?.SetInteger("State4", 2);
        animator?.SetInteger("State5", 2);

        isAttacking = true;
        lastAttackTime = Time.time;

        if (!string.IsNullOrEmpty(attackAnimationTrigger) && animator)
            animator.SetTrigger(attackAnimationTrigger); // 🧨 Триггер из инспектора

        Invoke(nameof(DoDamage), 0.3f);
        Invoke(nameof(ResetAttack), attackCooldown);
    }

    void DoDamage()
    {
        if (player != null && Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            PlayerStats stats = player.GetComponent<PlayerStats>();
            stats?.TakeDamage(damage);

            if (audioSource && attackSound)
                audioSource.PlayOneShot(attackSound);
        }
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
