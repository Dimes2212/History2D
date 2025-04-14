//using System.Collections;
//using UnityEngine;

//public class PlayerShooting : MonoBehaviour
//{
//    public GameObject bulletPrefab;       // Префаб пули
//    public Transform firePoint;           // Точка, откуда появляется пуля
//    public float shootCooldown = 0.5f;    // Задержка между выстрелами

//    [Header("Audio Settings")]
//    public AudioSource audioSource;       // Источник звука
//    public AudioClip shootSound;          // Звук выстрела
//    [SerializeField] private float shootSoundDuration = 0.5f; // Ограничение времени для звука (настраиваемое в инспекторе)
//    [SerializeField] private float shootVolume = 1f; // Уровень громкости звука (настраиваемое в инспекторе)

//    private float shootTimer = 0f;
//    private Animator animator;
//    private PlayerStats stats;

//    void Start()
//    {
//        animator = GetComponent<Animator>();
//        stats = GetComponent<PlayerStats>();
//    }

//    void Update()
//    {
//        shootTimer += Time.deltaTime;

//        // Стрельба по нажатию клавиши F
//        if (Input.GetKeyDown(KeyCode.F) && shootTimer >= shootCooldown)
//        {
//            // Стреляем только если есть патроны
//            if (stats != null && stats.UseAmmo())
//            {
//                Shoot();
//                shootTimer = 0f;
//            }
//            else
//            {
//                Debug.Log("Нет патронов!");
//            }
//        }
//    }

//    void Shoot()
//    {
//        // Воспроизведение анимации стрельбы
//        if (animator != null)
//            animator.SetTrigger("Shoot");

//        // Создание пули
//        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

//        // Направление пули: вправо или влево
//        int direction = transform.localScale.x > 0 ? 1 : -1;

//        Bullet bulletScript = bullet.GetComponent<Bullet>();
//        if (bulletScript != null)
//            bulletScript.SetDirection(direction);

//        // 🔊 Воспроизведение звука выстрела
//        if (audioSource && shootSound)
//        {
//            audioSource.volume = shootVolume;  // Устанавливаем громкость
//            audioSource.PlayOneShot(shootSound);
//            // Останавливаем звук после заданного времени
//            StartCoroutine(StopShootSoundAfterDelay());
//        }
//    }

//    // Ограничение времени для проигрывания звука
//    private IEnumerator StopShootSoundAfterDelay()
//    {
//        yield return new WaitForSeconds(shootSoundDuration);
//        audioSource.Stop();  // Останавливаем звук после задержки
//    }
//}


using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;       // Префаб пули
    public Transform firePoint;           // Точка, откуда появляется пуля
    public float shootCooldown = 0.5f;    // Задержка между выстрелами

    [Header("Audio Settings")]
    public AudioClip shootSound;          // Звук выстрела
    [SerializeField] private float shootVolume = 1f; // Уровень громкости звука (настраиваемое в инспекторе)

    private float shootTimer = 0f;
    private Animator animator;
    private PlayerStats stats;

    void Start()
    {
        animator = GetComponent<Animator>();
        stats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        shootTimer += Time.deltaTime;

        // Стрельба по нажатию клавиши F
        if (Input.GetKeyDown(KeyCode.F) && shootTimer >= shootCooldown)
        {
            // Стреляем только если есть патроны
            if (stats != null && stats.UseAmmo())
            {
                Shoot();
                shootTimer = 0f;
            }
            else
            {
                Debug.Log("Нет патронов!");
            }
        }
    }

    void Shoot()
    {
        // Воспроизведение анимации стрельбы
        if (animator != null)
            animator.SetTrigger("Shoot");

        // Создание пули
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Направление пули: вправо или влево
        int direction = transform.localScale.x > 0 ? 1 : -1;

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
            bulletScript.SetDirection(direction);

        // 🔊 Воспроизведение звука стрельбы (временный AudioSource)
        PlayShootSound();
    }

    void PlayShootSound()
    {
        if (shootSound == null) return;

        GameObject tempGO = new GameObject("TempShootAudio");
        tempGO.transform.position = transform.position;

        AudioSource tempSource = tempGO.AddComponent<AudioSource>();
        tempSource.clip = shootSound;
        tempSource.volume = shootVolume;
        tempSource.Play();

        Destroy(tempGO, shootSound.length);
    }
}
