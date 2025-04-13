using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    public float climbSpeed = 4f;
    private bool isClimbing = false;

    private Rigidbody2D rb;
    private float inputY;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        inputY = Input.GetAxisRaw("Vertical");

        // Если мы на лестнице — лезем
        if (isClimbing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, inputY * climbSpeed);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = true;
            rb.gravityScale = 0;
            rb.linearVelocity = Vector2.zero;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = false;
            rb.gravityScale = 1;
        }
    }
}
