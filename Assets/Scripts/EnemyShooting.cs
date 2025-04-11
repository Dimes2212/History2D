using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public Transform player;
    public Transform firePoint;
    public GameObject bulletPrefab;

    public float detectionRadius = 10f;
    public float fireRate = 1f;
    public float bulletForce = 10f;
    public LayerMask obstacleMask;

    private float fireCooldown;

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius && CanSeePlayer())
        {
            if (fireCooldown <= 0f)
            {
                Shoot();
                fireCooldown = 1f / fireRate;
            }
        }

        fireCooldown -= Time.deltaTime;
    }

    bool CanSeePlayer()
    {
        // Проверка прямой видимости с помощью Raycast
        Vector2 direction = (player.position - firePoint.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, detectionRadius, ~obstacleMask);

        return hit.collider != null && hit.collider.transform == player;
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * bulletForce, ForceMode2D.Impulse);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
