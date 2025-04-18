using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyControl : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float waypointRadius = 0.5f;

    [Header("Sprite Settings")]
    [SerializeField] private bool flipSpriteDirection = false;
    [SerializeField] private float rotationOffset = 180f;

    [Header("Collision")]
    [SerializeField] private float wallCheckDistance = 0.2f;
    [SerializeField] private LayerMask obstacleLayer;

    private Rigidbody2D rb;
    private int currentTargetIndex = 0;
    private Vector3 baseScale;
    private bool isMovingRight;
    private Animator animator;

    public bool IsFacingRight => isMovingRight;
    public bool IsPatrolling { get; private set; } = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
    if (animator == null)
    {
        Debug.LogError("Animator component missing!");
    }
        rb.gravityScale = 0;
        rb.freezeRotation = true;
        baseScale = transform.localScale;
        ApplyInitialFlip();
        UpdateMovementDirection();
        
    }

    void FixedUpdate()
    {
        if (!IsPatrolling || patrolPoints.Length == 0) return;

        HandleMovement();
        CheckWaypointProximity();
    }

    void HandleMovement()
    {
         if (Physics2D.Raycast(transform.position, GetDirection(), wallCheckDistance, obstacleLayer))
    {
        SwitchTarget();
        return;
    }

    // Устанавливаем анимацию "идёт" (1 - предположим, что это состояние ходьбы)
    if (animator != null)
    {
        animator.SetInteger("EnemyState", 1);
        animator.SetInteger("EnemyState2", 1);
        animator.SetInteger("State3", 1);
            animator.SetInteger("State4", 1);

        }

    Vector2 targetPosition = patrolPoints[currentTargetIndex].position;
    Vector2 newPosition = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime);
    rb.MovePosition(newPosition);
    }

    void UpdateMovementDirection()
    {
        Vector2 direction = GetDirection();
        bool shouldFaceRight = direction.x > 0;

        if (shouldFaceRight != isMovingRight)
        {
            Flip();
        }
    }

    void ApplyInitialFlip()
    {
        if (flipSpriteDirection)
        {
            transform.Rotate(0, rotationOffset, 0);
            isMovingRight = !isMovingRight;
        }
    }

    public void Flip()
    {
        isMovingRight = !isMovingRight;
        transform.Rotate(0, rotationOffset, 0);
    }

    void CheckWaypointProximity()
    {
        if (Vector2.Distance(transform.position, patrolPoints[currentTargetIndex].position) < waypointRadius)
        {
            SwitchTarget();
        }
    }

    void SwitchTarget()
    {
        currentTargetIndex = (currentTargetIndex + 1) % patrolPoints.Length;
        UpdateMovementDirection();
    }

    Vector2 GetDirection()
    {
        return (patrolPoints[currentTargetIndex].position - transform.position).normalized;
    }

    public void StopPatrol()
    {
         IsPatrolling = false;
    rb.linearVelocity = Vector2.zero;
    if (animator != null)
    {
        animator.SetInteger("EnemyState", 0); // 0 - состояние покоя
        animator.SetInteger("EnemyState2", 0);
        animator.SetInteger("State3", 0);
            animator.SetInteger("State4", 0);

        }
    }

    public void ResumePatrol()
    {
        IsPatrolling = true;
        UpdateMovementDirection();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        foreach (var point in patrolPoints)
        {
            if (point != null)
                Gizmos.DrawWireSphere(point.position, waypointRadius);
        }
        Gizmos.DrawRay(transform.position, GetDirection() * wallCheckDistance);
    }
}