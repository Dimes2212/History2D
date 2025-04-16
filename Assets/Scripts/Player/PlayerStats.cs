using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public int maxAmmo = 10;
    public int currentAmmo;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI ammoText;

    public Transform respawnPoint; // 👉 теперь здесь

    public event System.Action OnDamageTaken;

    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        currentAmmo = maxAmmo;
        UpdateHUD();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        UpdateHUD();
        OnDamageTaken?.Invoke();

        if (currentHealth == 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Игрок погиб");
        Respawn();
    }

    void Respawn()
    {
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;
        }

        RestoreFullHealth();
    }

    public void RestoreFullHealth()
    {
        isDead = false;
        currentHealth = maxHealth;
        currentAmmo = maxAmmo;
        UpdateHUD();
    }

    public bool UseAmmo()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
            UpdateHUD();
            return true;
        }
        return false;
    }

    public void AddAmmo(int amount)
    {
        currentAmmo += amount;
        if (currentAmmo > maxAmmo)
            currentAmmo = maxAmmo;
        UpdateHUD();
    }

    void UpdateHUD()
    {
        if (healthText != null)
            healthText.text = "X" + currentHealth.ToString();
        if (ammoText != null)
            ammoText.text = "X" + currentAmmo.ToString();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        UpdateHUD();
    }

}
