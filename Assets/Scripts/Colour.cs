using UnityEngine;

public class ChangeColorOnBulletHit : MonoBehaviour
{
    public Color hitColor = Color.red; // ����, �� ������� ������ ������ ��� ���������
    public float colorChangeDuration = 0.2f; // �����, ����� ������� ���� �������� � ���������

    private SpriteRenderer spriteRenderer;
    private Color originalColor; // �������� ���� �������

    void Start()
    {
        // �������� ��������� SpriteRenderer � ��������� �������� ����
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // ���������, ����� �� ������ ��� "Bullet"
        if (collision.gameObject.CompareTag("Bullet"))
        {
            ChangeColorTemporarily();
        }
    }

   

    void ChangeColorTemporarily()
    {
        if (spriteRenderer != null)
        {
            // �������� ���� �� hitColor
            spriteRenderer.color = hitColor;

            // ��������� �������� ��� �������� ����� �������
            StartCoroutine(ResetColorAfterDelay());
        }
    }

    System.Collections.IEnumerator ResetColorAfterDelay()
    {
        // ��������� �������� �����
        yield return new WaitForSeconds(colorChangeDuration);

        // ������� �������� ����
        spriteRenderer.color = originalColor;
    }
}
