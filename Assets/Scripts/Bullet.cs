using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 3f;

    private int direction = 1;

    public void SetDirection(int dir)
    {
        direction = dir;
        // Отражение пули, если нужно
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    void Start()
    {
        // Уничтожить пулю через lifetime секунд
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Движение по прямой (горизонтально)
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }
}
