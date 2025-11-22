using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform m_Start;
    [SerializeField] private float m_OrthoSize;
    [SerializeField] private CameraSetup m_IntroSetup;
    [SerializeField] private CameraSetup m_GameplaySetup;
    [SerializeField] private CameraSetup m_KOSetup;
    [SerializeField] private CameraSetup m_ResultSetup;

    private Camera m_MainCamera;
    private CameraMode m_CameraMode;
    private Vector3 m_StartPosition;
    private Quaternion m_StartRotation;
    private float m_StartOrthoSize;
    private float m_Time;

    private void Start()
    {
        m_MainCamera = Camera.main;
        StartSetup();
        SetMode(CameraMode.INTRO); // TEST
    }

    private void Update() => HandleCamera();

    private void HandleCamera()
    {
        switch (m_CameraMode)
        {
            case CameraMode.INTRO:
                MoveCamera(m_IntroSetup);
                break;

            case CameraMode.GAMEPLAY:
                MoveCamera(m_GameplaySetup);
                break;

            case CameraMode.KO:
                MoveCamera(m_KOSetup);
                break;

            case CameraMode.RESULT:
                MoveCamera(m_ResultSetup);
                break;
        }
    }

    private void StartSetup()
    {
        m_MainCamera.transform.position = m_Start.position;
        m_MainCamera.orthographicSize = m_OrthoSize;
    }

    public void SetMode(CameraMode newMode)
    {
        m_Time = 0;
        m_StartPosition = m_MainCamera.transform.position;
        m_StartRotation = m_MainCamera.transform.rotation;
        m_StartOrthoSize = m_MainCamera.orthographicSize;
        m_CameraMode = newMode;
    }

    private void MoveCamera(CameraSetup setup)
    {
        if (setup.target == null) return;

        m_Time += Time.deltaTime;

        var time = Mathf.Clamp01(m_Time / setup.duration);

        m_MainCamera.transform.SetPositionAndRotation(
            Vector3.Lerp(m_StartPosition, setup.target.position, setup.curve.Evaluate(time)),
            Quaternion.Slerp(m_StartRotation, setup.target.rotation, setup.curve.Evaluate(time))
            );

        m_MainCamera.orthographicSize = 
            Mathf.Lerp(m_StartOrthoSize, setup.orthoSize, setup.curve.Evaluate(time));
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