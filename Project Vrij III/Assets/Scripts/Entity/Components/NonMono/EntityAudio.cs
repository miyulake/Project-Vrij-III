using UnityEngine;
using Game.Entities;

public class EntityAudio : EntityContext, IEntityComponent
{
    private AudioData m_Data;
    private AudioSource m_AudioSource;

    public void Initialize(Entity entity)
    {
        SetEntity(entity);
        m_Data = Entity.Character.audio;
        m_AudioSource = Entity.GetComponent<AudioSource>();
    }

    public void GetPlay(SoundType sound, float volume = 1f)
    {
        var clip = m_Data.Get(sound);
        if (clip) m_AudioSource.PlayOneShot(clip, volume);
    }

    public void Play(AudioClip clip)
    {
        if (clip) m_AudioSource.PlayOneShot(clip);
    }

}