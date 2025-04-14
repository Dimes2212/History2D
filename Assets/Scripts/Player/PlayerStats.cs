using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;      // Максимальное здоровье
    public int currentHealth;        // Текущее здоровье

    public int maxAmmo = 10;         // Максимальное количество патронов
    public int currentAmmo;          // Текущее количество патронов

    public TextMeshProUGUI healthText; // UI элемент для здоровья
    public TextMeshProUGUI ammoText;   // UI элемент для патронов

    void Start()
    {
        currentHealth = maxHealth;   // Инициализация здоровья
        currentAmmo = maxAmmo;       // Инициализация патронов
        UpdateHUD();                  // Обновление HUD
    }

    // Метод для получения урона
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;     // Уменьшаем здоровье на указанное количество
        if (currentHealth < 0) currentHealth = 0;  // Убеждаемся, что здоровье не станет меньше 0

        UpdateHUD();  // Обновляем отображение здоровья

        if (currentHealth == 0)
        {
            Die();  // Если здоровье 0, игрок погибает
        }
    }

    // Метод для использования патронов
    public bool UseAmmo()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;   // Уменьшаем количество патронов
            UpdateHUD();      // Обновляем отображение патронов
            Debug.Log("Выстрел! Осталось патронов: " + currentAmmo);
            return true;
        }

        Debug.Log("Патроны закончились!"); // Сообщение, если патроны закончились
        return false;
    }

    // Метод, который срабатывает при смерти игрока
    void Die()
    {
        Debug.Log("Игрок погиб");
        // Здесь можно добавить анимацию смерти, рестарт, экран проигрыша и т.п.
    }

    // Функция для обновления HUD
    void UpdateHUD()
    {
        if (healthText != null)
            healthText.text = "X" + currentHealth.ToString();  // Только число

        if (ammoText != null)
            ammoText.text = "X" + currentAmmo.ToString();      // Только число
    }

    // Дополнительный метод для восстановления здоровья
    public void RestoreFullHealth()
    {
        currentHealth = maxHealth;
        currentAmmo = maxAmmo;
        UpdateHUD();
    }

    // Метод для добавления патронов
    public void AddAmmo(int amount)
    {
        currentAmmo += amount;
        if (currentAmmo > maxAmmo)
            currentAmmo = maxAmmo;
        UpdateHUD(); // Если HUD обновляется, обновите отображение
    }
}
