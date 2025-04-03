using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{

    public GameObject bulletPrefab; // ������ ����
    public Transform firePoint;     // ����� ��������

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // �������� ��� ������� �������
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.transform.position = transform.position; // �������� ����������� ������ ����
    }
}