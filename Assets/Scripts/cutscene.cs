using UnityEngine;

public class cutscene : MonoBehaviour
{
    public AudioClip soundClip;
    public float volume = 1f;
    public bool playOnStart = true;
    public bool loop = false;
    public float playDuration = 1f; // Сколько секунд звук должен проигрываться

    private AudioSource source;

    void Start()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.clip = soundClip;
        source.volume = volume;
        source.loop = loop;

        if (playOnStart && soundClip != null)
        {
            PlaySound();
        }
    }

    public void PlaySound()
    {
        if (soundClip == null)
        {
            Debug.LogWarning("CustomSoundPlayer: Звук не назначен.");
            return;
        }

        source.Play();

        // Останавливаем воспроизведение через заданное время независимо от loop
        Invoke(nameof(StopSound), playDuration);
    }

    public void StopSound()
    {
        if (source.isPlaying)
            source.Stop();

        CancelInvoke(nameof(StopSound));
    }
}
