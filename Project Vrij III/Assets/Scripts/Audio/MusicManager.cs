using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip[] music;
    private int musicIndex = -1;

    private void Awake() => Instance = this;

    private void Start() => SetRandomMusic();

    public void SetRandomMusic()
    {
        if (music.Length == 0) return;

        if (music.Length == 1) // Avoid the loop
        {
            musicIndex = 0;
            musicSource.clip = music[0];
            musicSource.Play();
            return;
        }

        int newIndex;
        do newIndex = Random.Range(0, music.Length);
        while (newIndex == musicIndex);

        musicIndex = newIndex;

        musicSource.clip = music[musicIndex];
        musicSource.Play();
    }
}