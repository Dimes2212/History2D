using UnityEngine;

public class EnemySword : MonoBehaviour
{
    public int damage = 10;
    private bool canDamage = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canDamage && other.CompareTag("Player"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
                Debug.Log("Игрок получил урон от сабли врага!");
            }
        }
    }

    public void EnableDamage() => canDamage = true;
    public void DisableDamage() => canDamage = false;
}
