using UnityEngine;

namespace Miyu.Tools
{
    public sealed class Tween
    {
        private readonly AnimationCurve _Curve;
        private readonly float _Duration;
        private readonly bool _Loop;

        private float m_ElapsedTime;
        private bool m_IsPlaying;

        public event System.Action<float> OnUpdate; // Called with normalized curve value (0-1)
        public event System.Action OnCompleted;

        public bool IsPlaying => m_IsPlaying;

        public Tween(AnimationCurve curve, float duration = 1f, bool loop = true)
        {
            if (duration <= 0f) throw new System.ArgumentOutOfRangeException(nameof(duration), "Duration must be > 0");
            _Curve = curve ?? throw new System.ArgumentNullException(nameof(curve), "Curve does not exist");
            _Duration = duration;
            _Loop = loop;
        }

        public void Tick(float deltaTime)
        {
            if (!m_IsPlaying) return;

            m_ElapsedTime += deltaTime;

            var normalizedTime = _Loop ? m_ElapsedTime / _Duration % 1f : Mathf.Clamp01(m_ElapsedTime / _Duration);
            var curveValue = _Curve.Evaluate(normalizedTime);
            OnUpdate?.Invoke(curveValue);

            if (!_Loop && m_ElapsedTime >= _Duration)
            {
                m_IsPlaying = false;
                OnCompleted?.Invoke();
            }
        }

        public void Play()
        {
            m_ElapsedTime = 0f;
            m_IsPlaying = true;
        }

        public void Randomize() => m_ElapsedTime = UnityEngine.Random.Range(0, _Duration);

        public void Stop() => m_IsPlaying = false;

        public void Reset()
        {
            m_ElapsedTime = 0f;
            m_IsPlaying = false;
        }
    }
}