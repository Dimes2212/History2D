using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints; // Точки патруля
    public float speed = 2f;         // Скорость движения
    public float waitTime = 2f;      // Время ожидания на точке

    private int currentPointIndex = 0;
    private float waitCounter = 0f;
    private bool isWaiting = false;

    void Update()
    {
        if (patrolPoints.Length == 0) return;

        if (isWaiting)
        {
            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0f)
            {
                isWaiting = false;
                currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            }
        }
        else
        {
            Transform targetPoint = patrolPoints[currentPointIndex];
            transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

            // Если дошли до точки
            if (Vector2.Distance(transform.position, targetPoint.position) < 0.2f)
            {
                transform.position = targetPoint.position; // Подровняем позицию
                isWaiting = true;
                waitCounter = waitTime;
            }

        }
    }

    // Чтобы увидеть патрульные точки в редакторе
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        if (patrolPoints != null)
        {
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                if (patrolPoints[i] != null)
                    Gizmos.DrawWireSphere(patrolPoints[i].position, 0.2f);

                if (i < patrolPoints.Length - 1 && patrolPoints[i] != null && patrolPoints[i + 1] != null)
                    Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
            }
        }
    }
}
