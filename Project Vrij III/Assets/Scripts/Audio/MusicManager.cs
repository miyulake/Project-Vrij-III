using UnityEngine;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip[] music;

    private readonly Queue<int> lastPlayed = new Queue<int>();
    private const int MemorySize = 2;

    private void Awake() => Instance = this;

    private void Start() => SetRandomMusic();

    public void SetRandomMusic()
    {
        if (music.Length == 0) return;

        if (music.Length <= MemorySize)
        {
            PlayMusic(Random.Range(0, music.Length));
            return;
        }

        int newIndex;
        do newIndex = Random.Range(0, music.Length);
        while (lastPlayed.Contains(newIndex));

        PlayMusic(newIndex);
    }

    private void PlayMusic(int index)
    {
        musicSource.clip = music[index];
        musicSource.Play();

        lastPlayed.Enqueue(index);
        if (lastPlayed.Count > MemorySize) lastPlayed.Dequeue();
    }
}
