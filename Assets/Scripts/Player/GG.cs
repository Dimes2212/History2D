using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GG : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    public float decelerationRate = 10f;
    private bool isGrounded = false;
    private Rigidbody2D rb;
    private Vector3 originalScale;
    private Animator animator;

    public AudioSource audioSource;           // Источник звука
    public AudioClip footstepSound;           // Звук шага
    public float stepInterval = 0.4f;         // Интервал между шагами
    private float stepTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");

        // Движение
        transform.position += new Vector3(moveX, 0, 0) * speed * Time.deltaTime;

        // Поворот
        if (moveX != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveX) * Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }

        // Анимации
        HandleAnimations(moveX);

        // Прыжок
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            Jump();
        }

        // Звук шагов
        HandleFootstepSound(moveX);
    }

    void HandleAnimations(float moveX)
    {
        if (isGrounded)
        {
            if (moveX != 0)
                animator.SetInteger("State", 1); // Бег
            else
                animator.SetInteger("State", 0); // Стоит
        }
        else
        {
            animator.SetInteger("State", 2); // Прыжок
        }
    }

    void HandleFootstepSound(float moveX)
    {
        if (isGrounded && Mathf.Abs(moveX) > 0.1f)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                audioSource.PlayOneShot(footstepSound);
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
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
