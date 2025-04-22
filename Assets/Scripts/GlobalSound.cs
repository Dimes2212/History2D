using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalSound : MonoBehaviour
{
    public static GlobalSound Instance;

    public AudioClip levelMusic;
    [Range(0f, 1f)] public float musicVolume = 0.5f;

    private AudioSource musicSource;

    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneUnloaded += OnSceneUnloaded; // Подписка на событие
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.clip = levelMusic;
        musicSource.volume = musicVolume;
        musicSource.loop = true;
        musicSource.playOnAwake = false;

        musicSource.Play();
    }

    private void OnDestroy()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded; // Отписка от события
    }

    // Когда сцена выгружается
    private void OnSceneUnloaded(Scene current)
    {
        StopMusic();
    }

    public void SetVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void StopMusic()
    {
        if (musicSource != null)
            musicSource.Stop();
    }

    public void PlayMusic()
    {
        if (musicSource != null && !musicSource.isPlaying)
            musicSource.Play();
    }
    public void ChangeMusic(AudioClip newClip, float volume = 0.5f)
    {
        if (musicSource.clip == newClip) return; // Уже играет нужный трек

        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.volume = volume;
        musicSource.Play();
    }

}
