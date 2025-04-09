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

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        shootTimer += Time.deltaTime;

        // Стрельба по нажатию клавиши F
        if (Input.GetKeyDown(KeyCode.F) && shootTimer >= shootCooldown)
        {
            Shoot();
            shootTimer = 0f;
        }
    }

    void Shoot()
    {
        // Воспроизведение анимации стрельбы
        animator.SetTrigger("Shoot");

        // Создание пули
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Определение направления (1 — вправо, -1 — влево)
        int direction = transform.localScale.x > 0 ? 1 : -1;

        // Установить направление пули
        bullet.GetComponent<Bullet>().SetDirection(direction);
    }
}
