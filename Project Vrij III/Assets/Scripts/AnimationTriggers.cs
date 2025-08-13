using UnityEngine;

public class AnimationTriggers : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    public void PlaySound(AudioClip audioClip) => audioSource.PlayOneShot(audioClip);
    public void SetAudioPitch(float pitch) => audioSource.pitch = pitch;
    public void SetSpeedMultiplier(float speed) => animator.SetFloat("SpeedMultiplier", speed);
}
