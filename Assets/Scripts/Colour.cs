using UnityEngine;

public class ChangeColorOnBulletHit : MonoBehaviour
{
    public Color hitColor = Color.red; // Цвет, на который объект меняет при попадании
    public float colorChangeDuration = 0.2f; // Время, через которое цвет вернется к исходному

    private SpriteRenderer spriteRenderer;
    private Color originalColor; // Исходный цвет объекта

    void Start()
    {
        // Получить компонент SpriteRenderer и сохранить исходный цвет
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверить, имеет ли объект тег "Bullet"
        if (collision.gameObject.CompareTag("Bullet"))
        {
            ChangeColorTemporarily();
        }
    }

   

    void ChangeColorTemporarily()
    {
        if (spriteRenderer != null)
        {
            // Изменить цвет на hitColor
            spriteRenderer.color = hitColor;

            // Запустить корутину для возврата цвета обратно
            StartCoroutine(ResetColorAfterDelay());
        }
    }

    System.Collections.IEnumerator ResetColorAfterDelay()
    {
        // Подождать заданное время
        yield return new WaitForSeconds(colorChangeDuration);

        // Вернуть исходный цвет
        spriteRenderer.color = originalColor;
    }
}
