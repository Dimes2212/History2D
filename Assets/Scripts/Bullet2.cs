using UnityEngine;
using System.Collections.Generic;

public class BulletCollision : MonoBehaviour
{
    private static Dictionary<GameObject, int> collisionCounts = new Dictionary<GameObject, int>();
    public static int collisionThreshold = 5;

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject target = collision.gameObject;

        // ���������, ����� �� ������ ��� "Ground"
        if (target.CompareTag("Ground") || target.CompareTag("Player"))
        {
            // ���� ������ ����� ��� "Ground", ������ �� ������
            return;
        }

        // ���������, ���� �� ��� ������ �� ���� �������
        if (!collisionCounts.ContainsKey(target))
        {
            collisionCounts[target] = 0;
        }

        // ��������� ������� ������������ ��� ����� �������
        collisionCounts[target]++;

        // ���������, �������� �� ���������� ������������ ���������� ��������
        if (collisionCounts[target] >= collisionThreshold)
        {
            // ���������� ������ � ������� ��� �� �������
            Destroy(target);
            collisionCounts.Remove(target);
        }

        Destroy(gameObject);
    }
}

  
