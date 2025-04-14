using UnityEngine;

public class Bullet : MonoBehaviour
{
    public void SetDirection(int direction, float speed)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.freezeRotation = true;
            rb.linearVelocity = new Vector2(direction, 0f) * speed;
        }
    }
}
