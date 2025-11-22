using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    [SerializeField] private Transform m_Start;
    [SerializeField] private float m_OrthoSize;

    [SerializeField] private CameraSetup m_IntroSetup;
    [SerializeField] private CameraSetup m_KnockoutSetup;
    [SerializeField] private CameraSetup m_ResultSetup;

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

    private void Update() => HandleCamera();

    private void HandleCamera()
    {
        switch (RoundManager.Instance.CurrentState)
        {
            case RoundState.INTRO:
                MoveCamera(m_IntroSetup);
                break;

            case RoundState.GAMEPLAY:
                
                break;

            case RoundState.KNOCKOUT:
                MoveCamera(m_KnockoutSetup);
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

    private void MoveCamera(CameraSetup setup)
    {
        m_Time += Time.deltaTime;

        var time = Mathf.Clamp01(m_Time / setup.duration);

        if (setup.target != null)
        {
            var targetPosition = Vector3.Lerp(m_StartPosition, setup.target.position, setup.curve.Evaluate(time));
            var targetRotation = Quaternion.Slerp(m_StartRotation, setup.target.rotation, setup.curve.Evaluate(time));
            m_MainCamera.transform.SetPositionAndRotation(targetPosition, targetRotation);
        }

        m_MainCamera.orthographicSize = Mathf.Lerp(m_StartOrthoSize, setup.orthoSize, setup.curve.Evaluate(time));
    }

    [System.Serializable]
    private struct CameraSetup
    {
        public Transform target;
        public float orthoSize;
        public AnimationCurve curve;
        public float duration;
    }
}