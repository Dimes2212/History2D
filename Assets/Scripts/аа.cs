using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    private Rigidbody2D rb;
    public bool isGrounded;
    private Vector2 lastMovementDirection = Vector2.right; // Initially moving to the right

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(horizontalInput, 0); //Remove vertical input from horizontal movement
        rb.linearVelocity = new Vector2(movement.x * speed, rb.linearVelocity.y); //Only control horizontal velocity

        //Store last movement direction
        if (movement.magnitude > 0)
        {
            lastMovementDirection = movement.normalized;
        }

        //Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    public Vector2 GetLastMovementDirection()
    {
        return lastMovementDirection;
    }
}
