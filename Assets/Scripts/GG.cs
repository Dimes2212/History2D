//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GG : MonoBehaviour
//{
//    public float speed = 5.0f;
//    public float jumpForce = 5.0f;
//    public float decelerationRate = 10f; // �������� ����������
//    private bool isGrounded = false;
//    private Rigidbody2D rb;
//    private Vector3 originalScale;


//    private Animator animator;


//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        originalScale = transform.localScale;
//        animator = GetComponent<Animator>();
//    }


//    [System.Obsolete]
//    void FixedUpdate()
//    {
//        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
//        {
//            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
//            isGrounded = false;
//        }

//        /*float horizontalInput = Input.GetAxis("Horizontal");
//        Vector3 velocity = rb.linearVelocity;
//        velocity.x = horizontalInput * speed;
//        rb.linearVelocity = velocity;

//        if (Mathf.Abs(horizontalInput) < 0.01f)
//        {
//            velocity.x = Mathf.MoveTowards(velocity.x, 0, decelerationRate * Time.fixedDeltaTime);
//            rb.linearVelocity = velocity;
//        }*/
//        // Получить горизонтальный ввод (клавиши A/D или стрелки влево/вправо)
//        float moveX = Input.GetAxis("Horizontal");

//        // Переместить объект только по оси X
//        transform.position += new Vector3(moveX, 0, 0) * speed * Time.deltaTime;





//        if (Input.GetAxis("Horizontal") != 0)
//        {
//            if (Input.GetAxis("Horizontal") < 0) 
//            {
//                transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
//            }
//            else if (Input.GetAxis("Horizontal") > 0) 
//            {
//                transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
//            }

//            animator.SetInteger("State", 1);
//        }
//        else
//        {
//            animator.SetInteger("State", 0);
//        }

//    }

//    void OnCollisionEnter2D(Collision2D collision)
//    {

//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            isGrounded = true;
//        }
//    }

//    void OnCollisionExit2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            isGrounded = false;
//        }
//    }

//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GG : MonoBehaviour
{
    public float speed = 5.0f;  // Скорость движения
    public float jumpForce = 5.0f;  // Сила прыжка
    public float decelerationRate = 10f;  // Рейтинг замедления
    private bool isGrounded = false;  // Переменная для проверки на земле
    private Rigidbody2D rb;
    private Vector3 originalScale;
    private Animator animator;  // Для анимаций

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
        animator = GetComponent<Animator>();  // Инициализация аниматора
    }

    void Update()
    {
        // Проверка нажатия для прыжка (Space или W)
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            Jump();
        }

        // Управление движением по горизонтали
        float moveX = Input.GetAxis("Horizontal");

        // Перемещение по оси X
        transform.position += new Vector3(moveX, 0, 0) * speed * Time.deltaTime;

        // Поворот персонажа в зависимости от направления движения
        if (moveX != 0)
        {
            // Поворот персонажа влево или вправо
            transform.localScale = new Vector3(Mathf.Sign(moveX) * Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }

        // Управление анимациями
        HandleAnimations(moveX);
    }

    private void HandleAnimations(float moveX)
    {
        if (isGrounded)
        {
            if (moveX != 0)
            {
                animator.SetInteger("State", 1); // Бег
            }
            else
            {
                animator.SetInteger("State", 0); // Стояние
            }
        }
        else
        {
            animator.SetInteger("State", 2); // Прыжок
        }
    }

    // Метод для прыжка
    private void Jump()
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
