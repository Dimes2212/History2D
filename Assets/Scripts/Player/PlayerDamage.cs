using System.Collections;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public PlayerStats stats;              // Ссылка на скрипт характеристик игрока
    public SpriteRenderer spriteRenderer;  // Рендерер игрока, чтобы менять цвет
    public Color damageColor = Color.red;  // Цвет при получении урона
    public float colorDuration = 0.2f;     // Сколько игрок будет красным
    public Transform respawnPoint;         // Точка для возрождения

    private Color originalColor;

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Если объект, с которым столкнулся игрок — это пуля
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet != null)  // Проверяем, есть ли компонент Bullet
        {
            Destroy(collision.gameObject); // Уничтожаем пулю

            // Урон игроку
            stats.TakeDamage(1);

            // Визуальный эффект при попадании
            StartCoroutine(FlashDamageColor());

            // Если здоровье 0, телепортируем игрока
            if (stats.currentHealth <= 0)  // Используем currentHealth вместо Health
            {
                Respawn();
            }
        }
    }

    IEnumerator FlashDamageColor()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(colorDuration);
        spriteRenderer.color = originalColor;
    }

    void Respawn()
    {
        transform.position = respawnPoint.position;
        stats.RestoreFullHealth(); // Восстанавливаем здоровье при респавне
    }
}
