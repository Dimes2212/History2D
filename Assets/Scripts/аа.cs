using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 lastMovementDirection = Vector2.right; // Изначально двигаемся вправо

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(horizontalInput, 0);

        rb.linearVelocity = movement * speed;
        if (movement.magnitude > 0)
        {
            lastMovementDirection = movement.normalized; // Запоминаем последнее направление движения
        }
    }

    public Vector2 GetLastMovementDirection()
    {
        return lastMovementDirection;
    }
}