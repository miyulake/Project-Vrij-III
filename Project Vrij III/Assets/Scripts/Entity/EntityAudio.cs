using UnityEngine;

public class EntityAudio : EntityComponent
{
    [SerializeField] private AudioSource audioSource;

    public void Play(AudioClip clip) => audioSource.PlayOneShot(clip);
}