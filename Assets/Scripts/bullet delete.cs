using UnityEngine;

public class BulletDelete : MonoBehaviour
{
    public float lifetime = 5f;

    void Start()
    {
        // Уничтожаем пулю через заданное время на всякий случай
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(int direction, float speed)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
        }
    }
}
