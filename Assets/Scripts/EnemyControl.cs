using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyControl : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float waypointRadius = 0.5f;

    [Header("Collision Detection")]
    [SerializeField] private float wallCheckDistance = 0.2f;
    [SerializeField] private LayerMask obstacleLayer;

    private Rigidbody2D rb;
    private int currentTargetIndex = 0;
    private Vector3 baseScale;
    private bool isFacingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;

        // Фиксация оригинального масштаба
        baseScale = transform.localScale;

        // Принудительная установка масштаба
        transform.localScale = baseScale;

        UpdateMovementDirection();
    }

    void FixedUpdate()
    {
        if (patrolPoints.Length == 0) return;

        HandleMovement();
        CheckWaypointProximity();
        ForceCorrectScale();
    }

    void HandleMovement()
    {
        // Проверка препятствий
        if (Physics2D.Raycast(transform.position, GetDirection(), wallCheckDistance, obstacleLayer))
        {
            SwitchTarget();
            return;
        }

        // Плавное движение
        Vector2 targetPosition = patrolPoints[currentTargetIndex].position;
        Vector2 newPosition = Vector2.MoveTowards(
            rb.position,
            targetPosition,
            moveSpeed * Time.fixedDeltaTime
        );

        rb.MovePosition(newPosition);
    }

    void UpdateMovementDirection()
    {
        if (patrolPoints.Length == 0) return;

        Vector2 direction = GetDirection();

        // Обновление направления
        bool shouldFaceRight = direction.x > 0;
        if (shouldFaceRight != isFacingRight) Flip();
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

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 newScale = baseScale;
        newScale.x *= isFacingRight ? 1 : -1;
        transform.localScale = newScale;
    }

    Vector2 GetDirection()
    {
        return (patrolPoints[currentTargetIndex].position - transform.position).normalized;
    }

    void ForceCorrectScale()
    {
        // Защита от изменения масштаба
        if (transform.localScale != baseScale && transform.localScale != new Vector3(-baseScale.x, baseScale.y, baseScale.z))
        {
            transform.localScale = isFacingRight ? baseScale : new Vector3(-baseScale.x, baseScale.y, baseScale.z);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            foreach (var point in patrolPoints)
            {
                if (point != null)
                    Gizmos.DrawWireSphere(point.position, waypointRadius);
            }
        }

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, GetDirection() * wallCheckDistance);
    }
}