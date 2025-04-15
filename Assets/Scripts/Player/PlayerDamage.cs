using System.Collections;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    public PlayerStats stats;
    public SpriteRenderer spriteRenderer;
    public Color damageColor = Color.red;
    public float colorDuration = 0.2f;

    private Color originalColor;

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;

        if (stats != null)
            stats.OnDamageTaken += () => StartCoroutine(FlashDamageColor());
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            Destroy(collision.gameObject);
            stats.TakeDamage(1);
        }
    }

    IEnumerator FlashDamageColor()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(colorDuration);
        spriteRenderer.color = originalColor;
    }
}
