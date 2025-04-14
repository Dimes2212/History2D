using System.Collections;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public EnemyStats stats;
    public SpriteRenderer spriteRenderer;
    public Color damageColor = Color.red;
    public float colorDuration = 0.2f;

    [Header("Audio")]
    public AudioClip hitSound;                    // Звук попадания
    public AudioClip deathSound;                  // Звук смерти
    [SerializeField] private float hitSoundVolume = 1f;
    [SerializeField] private float deathSoundVolume = 1f;

    private Color originalColor;

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            Destroy(collision.gameObject);

            PlayHitSound();               // 💥 Звук попадания
            stats.TakeDamage(1);
            StartCoroutine(FlashDamageColor());

            if (stats.currentHealth <= 0)
            {
                Die();
            }
        }
    }

    IEnumerator FlashDamageColor()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(colorDuration);
        spriteRenderer.color = originalColor;
    }

    void PlayHitSound()
    {
        if (hitSound != null)
        {
            GameObject tempHitSound = new GameObject("TempHitSound");
            tempHitSound.transform.position = transform.position;

            AudioSource hitSource = tempHitSound.AddComponent<AudioSource>();
            hitSource.clip = hitSound;
            hitSource.volume = hitSoundVolume;
            hitSource.Play();

            Destroy(tempHitSound, hitSound.length);
        }
    }

    void Die()
    {
        Debug.Log("Враг погиб");

        if (deathSound != null)
        {
            GameObject tempDeathSound = new GameObject("TempDeathSound");
            tempDeathSound.transform.position = transform.position;

            AudioSource deathSource = tempDeathSound.AddComponent<AudioSource>();
            deathSource.clip = deathSound;
            deathSource.volume = deathSoundVolume;
            deathSource.Play();

            Destroy(tempDeathSound, deathSound.length);
        }

        Destroy(gameObject);
    }
}
