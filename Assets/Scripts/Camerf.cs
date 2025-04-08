using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    
    
    public Transform target; // Трансформ игрока
    public float smoothSpeed = 0.125f; // Скорость сглаживания
    public Vector3 offset; // Отступ камеры от игрока

        void LateUpdate()
        {   
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
        }
    

}
