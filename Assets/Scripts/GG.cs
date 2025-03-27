using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GG : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    public float decelerationRate = 10f; // �������� ����������
    private bool isGrounded = false;
    private Rigidbody2D rb;
    private Vector3 originalScale;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        // ������ � ��������� �� �����
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    [System.Obsolete]
    void FixedUpdate()
    {
        // �������� ���� �� �����������
        float horizontalInput = Input.GetAxis("Horizontal");

        // ������������� �������� ��������
        Vector2 velocity = rb.velocity;
        velocity.x = horizontalInput * speed;
        rb.velocity = velocity;

        // ���� ��� �����, �����������
        if (Mathf.Abs(horizontalInput) < 0.01f)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, decelerationRate * Time.fixedDeltaTime);
            rb.velocity = velocity;
        }

        // ������� ���������
        if (horizontalInput < 0) // �������� ������
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (horizontalInput > 0) // �������� �����
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // ��������� �������� � ������
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // ���������� ������ ��� ������ �� ��������
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}

