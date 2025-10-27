using UnityEngine;

public class ShakeController : MonoBehaviour
{
    private Transform shakeTarget;
    private float shakeDuration;
    private float shakeMagnitude;
    private float shakeTimer;
    private Vector3 originalPosition;

    private void Update()
    {
        if (shakeTimer > 0f && shakeTarget != null)
        {
            shakeTimer -= Time.deltaTime;

            var durationSafe = Mathf.Max(shakeDuration, 0.0001f); // Avoid zero
            var progress = Mathf.Clamp01(1f - (shakeTimer / durationSafe)); // progress from 0 -> 1
            var currentMagnitude = shakeMagnitude * Mathf.Sqrt(1f - progress);
            var offsetX = Random.Range(-1f, 1f) * currentMagnitude;
            var offsetY = Random.Range(-1f, 1f) * currentMagnitude;
            var shakeOffset = new Vector3(offsetX, offsetY, 0f);
            shakeTarget.position = originalPosition + shakeOffset;

            if (shakeTimer <= 0f) shakeTarget.position = originalPosition; // Reset
        }
    }

    public void TriggerShake(Transform target, float duration, float magnitude)
    {
        shakeTarget = target;
        shakeDuration = duration;
        shakeTimer = duration;
        shakeMagnitude = magnitude;
        originalPosition = target.position;
    }
}
