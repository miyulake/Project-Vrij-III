using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    [SerializeField] private Transform m_Start;
    [SerializeField] private float m_OrthoSize;

    [Header("Setups")]
    [SerializeField] private CameraSetup m_IntroSetup;
    [SerializeField] private CameraSetup m_KnockoutSetup;
    [SerializeField] private CameraSetup m_ResultSetup;

    [Header("Zoom Settings")]
    [SerializeField] float m_MinZoom = 5f;
    [SerializeField] float m_MaxZoom = 7f;
    [SerializeField] float m_MinDistance = 5f;
    [SerializeField] float m_MaxDistance = 10f;
    [SerializeField] float m_SmoothTime = 2f;
    private float m_ZoomVelocity;

    private Camera m_MainCamera;
    private Vector3 m_StartPosition;
    private Quaternion m_StartRotation;
    private float m_StartOrthoSize;
    private float m_Time;

    private void Awake()
    {
        Instance = this;
        m_MainCamera = Camera.main;
    }

    private void LateUpdate() => HandleCamera();

    private void HandleCamera()
    {
        switch (RoundManager.Instance.CurrentState)
        {
            case RoundState.INTRO:
                MoveCamera(m_IntroSetup, RoundManager.Instance.introDuration);
                break;

            case RoundState.GAMEPLAY:
                HandleCameraZoom(); // Zoom effect during gameplay
                break;

            case RoundState.KNOCKOUT:
                MoveCamera(m_KnockoutSetup, RoundManager.Instance.knockoutDuration);
                break;

            case RoundState.RESULT:
                MoveCamera(m_ResultSetup);
                break;
        }
    }

    public void SetStartSetup()
    {
        m_MainCamera.transform.position = m_Start.position;
        m_MainCamera.orthographicSize = m_OrthoSize;
    }

    public void ResetSetup()
    {
        m_Time = 0;
        m_StartPosition = m_MainCamera.transform.position;
        m_StartRotation = m_MainCamera.transform.rotation;
        m_StartOrthoSize = m_MainCamera.orthographicSize;
    }

    private void MoveCamera(CameraSetup setup, float duration = 2f)
    {
        m_Time += Time.deltaTime;

        var time = Mathf.Clamp01(m_Time / duration);

        if (setup.target != null)
        {
            var targetPosition = Vector3.Lerp(m_StartPosition, setup.target.position, setup.curve.Evaluate(time));
            var targetRotation = Quaternion.Slerp(m_StartRotation, setup.target.rotation, setup.curve.Evaluate(time));
            m_MainCamera.transform.SetPositionAndRotation(targetPosition, targetRotation);
        }

        m_MainCamera.orthographicSize = Mathf.Lerp(m_StartOrthoSize, setup.orthoSize, setup.curve.Evaluate(time));
    }

    private void HandleCameraZoom()
    {
        var playerOnePosition = PlayerManager.Instance.playerOne.transform.position;
        var playerTwoPosition = PlayerManager.Instance.playerTwo.transform.position;

        var distanceX = Mathf.Abs(playerOnePosition.x - playerTwoPosition.x);
        var clampedDistance = Mathf.Clamp(distanceX, m_MinDistance, m_MaxDistance);
        var normalizedDistance = (clampedDistance - m_MinDistance) / (m_MaxDistance - m_MinDistance);

        var targetZoom = Mathf.Lerp(m_MinZoom, m_MaxZoom, normalizedDistance);
        m_MainCamera.orthographicSize = Mathf.SmoothDamp(
            m_MainCamera.orthographicSize,
            targetZoom,
            ref m_ZoomVelocity,
            m_SmoothTime
        );
    }

    [System.Serializable]
    private struct CameraSetup
    {
        public Transform target;
        public float orthoSize;
        public AnimationCurve curve;
    }
}