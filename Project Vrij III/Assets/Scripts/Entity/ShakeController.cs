using Game.Entities;
using UnityEngine;

public class ShakeController : EntityComponent, ITickable
{
    [SerializeField] private Transform shakeTarget;
    private float shakeDuration;
    private float shakeMagnitude;
    private float shakeTimer;
    private Vector3 originalPosition;

    public void Tick()
    {
        if (shakeTarget == null) return;

        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;

            var durationSafe = Mathf.Max(shakeDuration, 0.0001f);
            var progress = Mathf.Clamp01(1f - (shakeTimer / durationSafe));
            var currentMagnitude = shakeMagnitude * Mathf.Sqrt(1f - progress);
            var offsetX = Random.Range(-1f, 1f) * currentMagnitude;
            var offsetY = Random.Range(-1f, 1f) * currentMagnitude;
            var shakeOffset = new Vector3(offsetX, offsetY, 0f);

            shakeTarget.localPosition = originalPosition + shakeOffset;
            if (shakeTimer <= 0f) shakeTarget.localPosition = originalPosition;
        }
    }

    public void TriggerShake(float duration, float magnitude)
    {
        if (shakeTarget == null) return;

        originalPosition = shakeTarget.localPosition;
        shakeDuration = duration;
        shakeTimer = duration;
        shakeMagnitude = magnitude;
    }
}
