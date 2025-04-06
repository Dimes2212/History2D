using UnityEngine;
using System.Collections.Generic;

public class BulletCollision : MonoBehaviour
{
    private static Dictionary<GameObject, int> collisionCounts = new Dictionary<GameObject, int>();
    public static int collisionThreshold = 5;

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject target = collision.gameObject;

        // Проверить, имеет ли объект тег "Ground"
        if (target.CompareTag("Ground") || target.CompareTag("Player"))
        {
            // Если объект имеет тег "Ground", ничего не делать
            return;
        }

        // Проверить, есть ли уже запись об этом объекте
        if (!collisionCounts.ContainsKey(target))
        {
            collisionCounts[target] = 0;
        }

        // Увеличить счетчик столкновений для этого объекта
        collisionCounts[target]++;

        // Проверить, достигло ли количество столкновений порогового значения
        if (collisionCounts[target] >= collisionThreshold)
        {
            // Уничтожить объект и удалить его из словаря
            Destroy(target);
            collisionCounts.Remove(target);
        }

        Destroy(gameObject);
    }
}

  
