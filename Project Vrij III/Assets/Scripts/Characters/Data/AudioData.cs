using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "CharacterSO/Unique/Audio Data", order = 0)]
public class AudioData : ScriptableObject
{
    [SerializeField] private SoundEntry[] sounds;
    private Dictionary<SoundType, AudioClip> _cache;

    public AudioClip Get(SoundType sound)
    {
        _cache ??= sounds.ToDictionary(s => s.sound, s => s.clip);
        return _cache.TryGetValue(sound, out var clip) ? clip : null;
    }
}