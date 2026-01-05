using UnityEngine;
using Game.Entities;

public class EntityAudio : EntityContext, IEntityComponent
{
    private AudioData m_Data;

    public void Initialize(Entity entity)
    {
        SetEntity(entity);
        m_Data = Entity.Character.audio;
    }

    public void GetPlay(SoundType sound, float volume = 1f)
    {
        var clip = m_Data.Get(sound);
        if (clip) ViewComp.AudioSource.PlayOneShot(clip, volume);
    }

    public void Play(AudioClip clip)
    {
        if (clip) ViewComp.AudioSource.PlayOneShot(clip);
    }

}