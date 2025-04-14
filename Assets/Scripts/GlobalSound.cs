using UnityEngine;

public class GlobalSound : MonoBehaviour
{
    public static GlobalSound Instance;

    public AudioClip levelMusic;
    [Range(0f, 1f)] public float musicVolume = 0.5f;

    private AudioSource musicSource;

    void Awake()
    {
        // Singleton — сохраняем один экземпляр
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Не уничтожать при переходе между сценами
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Создаем аудиоисточник
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = levelMusic;
        musicSource.volume = musicVolume;
        musicSource.loop = true;
        musicSource.playOnAwake = false;

        musicSource.Play();
    }

    public void SetVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayMusic()
    {
        if (!musicSource.isPlaying)
            musicSource.Play();
    }
}
