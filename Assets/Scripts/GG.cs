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


    private Animator animator;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        animator = GetComponent<Animator>();
    }


    [System.Obsolete]
    void FixedUpdate()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }

        /*float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 velocity = rb.linearVelocity;
        velocity.x = horizontalInput * speed;
        rb.linearVelocity = velocity;

        if (Mathf.Abs(horizontalInput) < 0.01f)
        {
            velocity.x = Mathf.MoveTowards(velocity.x, 0, decelerationRate * Time.fixedDeltaTime);
            rb.linearVelocity = velocity;
        }*/
        // Получить горизонтальный ввод (клавиши A/D или стрелки влево/вправо)
        float moveX = Input.GetAxis("Horizontal");

        // Переместить объект только по оси X
        transform.position += new Vector3(moveX, 0, 0) * speed * Time.deltaTime;





        if (Input.GetAxis("Horizontal") != 0)
        {
            if (Input.GetAxis("Horizontal") < 0) 
            {
                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }
            else if (Input.GetAxis("Horizontal") > 0) 
            {
                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
            }

            animator.SetInteger("State", 1);
        }
        else
        {
            animator.SetInteger("State", 0);
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

}

