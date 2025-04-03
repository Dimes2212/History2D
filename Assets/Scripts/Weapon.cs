using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{

    public GameObject bulletPrefab; // Префаб пули
    public Transform firePoint;     // Точка стрельбы

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // Стрельба при нажатии пробела
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.transform.position = transform.position; // Передаем направление игрока пуле
    }
}