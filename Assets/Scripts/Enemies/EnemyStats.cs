using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int maxHealth = 100;      // Максимальное здоровье
    public int currentHealth;        // Текущее здоровье

    public int maxAmmo = 10;         // Максимальное количество патронов
    public int currentAmmo;          // Текущее количество патронов

    [Header("Drop Settings")]
    public GameObject ammoBoxPrefab; // Префаб коробки с патронами

    void Start()
    {
        currentHealth = maxHealth;   // Инициализация здоровья
        currentAmmo = maxAmmo;       // Инициализация патронов
    }

    // Метод для получения урона
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;     // Уменьшаем здоровье на указанное количество
        if (currentHealth < 0) currentHealth = 0;  // Убеждаемся, что здоровье не станет меньше 0

        if (currentHealth == 0)
        {
            Die();  // Если здоровье 0, враг погибает
        }
    }

    // Метод для использования патронов
    public bool UseAmmo()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;   // Уменьшаем количество патронов
            return true;
        }

        Debug.Log("Патроны врага закончились!"); // Сообщение, если патроны закончились
        return false;
    }

    // Метод, который срабатывает при смерти врага
    void Die()
    {
        Debug.Log("Враг погиб");
        DropAmmoBox(); // Спавн коробки с патронами

        Destroy(gameObject);  // Уничтожение объекта (враг погиб)
    }

    // Спавн коробки с патронами
    void DropAmmoBox()
    {
        if (ammoBoxPrefab != null)
        {
            Instantiate(ammoBoxPrefab, transform.position, Quaternion.identity);
        }
    }
}
