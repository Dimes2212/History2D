using UnityEngine;

public class Bullet : MonoBehaviour
{
  


    public float speed;
    private Rigidbody2D rb;
    private Vector2 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // direction = transform.right; // ����������� ������ �� ���������

        // rb.velocity = direction * speed;

        // �������� ����������� �������� �� ������� �������, �������� �� ������� ������
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // ��� ������ �������� ������� ������

        if (player != null)
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                direction = playerMovement.GetLastMovementDirection(); // �������� ����������� �� ������� ������
            }
            else
            {
                Debug.LogError("PlayerMovement script not found on the player!");
            }
        }
        else
        {
            Debug.LogError("Player not found.  Make sure the player has the 'Player' tag.");
            direction = Vector2.right;
        }
        rb.linearVelocity = direction * speed; // ��������� ��������
    }

    void Update()
    {
        // ���������� ���� ����� ��������� ����� ��� ��� ������ �� ������� ������
        Destroy(gameObject, 5f); // ����������� ����� 5 ������
    }
}
