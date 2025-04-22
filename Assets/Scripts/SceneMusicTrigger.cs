using UnityEngine;

public class SceneMusicTrigger : MonoBehaviour
{
    public AudioClip sceneMusic;
    [Range(0f, 1f)] public float volume = 0.5f;

    void Start()
    {
        if (GlobalSound.Instance != null && sceneMusic != null)
        {
            GlobalSound.Instance.ChangeMusic(sceneMusic, volume);
        }
    }
}
