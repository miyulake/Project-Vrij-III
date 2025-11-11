using UnityEngine;

public class MusicRandomizer : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip[] music;
    private int musicIndex;

    private void Start() => SetMusic();

    private void SetMusic()
    {
        if (music.Length == 0) return;

        var newIndex = Random.Range(0, music.Length);
        musicIndex = newIndex;

        musicSource.clip = music[musicIndex];
        musicSource.Play();
    }
}