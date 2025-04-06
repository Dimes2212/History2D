using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // ��������� ������
    public float speed = 5f; // �������� �������� ������
    public Vector3 offset = new Vector3(0, 0, -10); // ������ ������ �� ������

    void LateUpdate()
    {
        // ������� ����������� ������ �� ��� X
        float newX = Mathf.Lerp(transform.position.x, target.position.x + offset.x, speed * Time.deltaTime);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
