using UnityEngine;
using Game.Entities;

public class EntityAudio : EntityContext, IEntityComponent
{
    public void Initialize(Entity entity) => SetEntity(entity);
    public void Play(AudioClip clip) => ViewComp.AudioSource.PlayOneShot(clip);
}