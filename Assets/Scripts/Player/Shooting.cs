using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;       // Префаб пули
    public Transform firePoint;           // Точка, откуда появляется пуля
    public float shootCooldown = 0.5f;    // Задержка между выстрелами

    private float shootTimer = 0f;
    private Animator animator;
    private PlayerStats stats;

    void Start()
    {
        animator = GetComponent<Animator>();
        stats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        shootTimer += Time.deltaTime;

        // Стрельба по нажатию клавиши F
        if (Input.GetKeyDown(KeyCode.F) && shootTimer >= shootCooldown)
        {
            // Стреляем только если есть патроны
            if (stats != null && stats.UseAmmo())
            {
                Shoot();
                shootTimer = 0f;
            }
            else
            {
                Debug.Log("Нет патронов!");
            }
        }
    }

    void Shoot()
    {
        // Воспроизведение анимации стрельбы
        if (animator != null)
            animator.SetTrigger("Shoot");

        // Создание пули
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Направление пули: вправо или влево
        int direction = transform.localScale.x > 0 ? 1 : -1;

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
            bulletScript.SetDirection(direction);
    }
}
