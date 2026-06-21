using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 2.0f;
    [SerializeField] private float returnDelay = 2.0f;
    [SerializeField] private float baseTilt = 40f;
    [SerializeField] private float maximumRotation = 180;

    private Quaternion m_baseRotation;
    private Quaternion m_originRotation;
    private Quaternion m_targetRotation;
    private Quaternion m_restRotation;

    private float m_dragYaw = 0f;           // accumulated horizontal drag
    private float m_dragPitch = 0f;         // accumulated vertical drag

    private float m_rotationStart = -1.0f;

    private Coroutine m_rotationCoroutine = null;

    private void Start()
    {
        m_baseRotation = Quaternion.Euler(baseTilt, 0f, 0f);
        transform.rotation = m_baseRotation;

        m_originRotation = transform.rotation;
        m_targetRotation = transform.rotation;
    }

    private IEnumerator _RotateTowards(float delayInSeconds = 0.0f)
    {
        if (!(m_originRotation == m_targetRotation || m_rotationStart < 0))
        {
            EventManager.TriggerEvent(Event.CAMERA_LOCK_EVENT, null);

            new WaitForSeconds(delayInSeconds);

            while (transform.rotation != m_targetRotation)
            {
                float t = (Time.time - m_rotationStart) * rotationSpeed;
                transform.rotation = Quaternion.Slerp(m_originRotation, m_targetRotation, t);

                yield return null;
            }

            m_originRotation = transform.rotation;
            m_rotationStart = -1.0f;

            EventManager.TriggerEvent(Event.CAMERA_UNLOCK_EVENT, null);
        }

        m_rotationCoroutine = null;
    }

    public void RotateTowards(Quaternion target)
    {
        m_originRotation = transform.rotation;
        m_targetRotation = target * m_baseRotation;
        m_rotationStart = Time.time;

        if (m_rotationCoroutine != null) 
            StopCoroutine(m_rotationCoroutine);

        m_rotationCoroutine = StartCoroutine(_RotateTowards());
    }

    public void Free()
    {
        m_restRotation = transform.rotation;
        m_dragYaw = 0;
        m_dragPitch = 0;
    }

    public void Rotate(Vector2 input)
    {
        if (m_rotationCoroutine != null)
            StopCoroutine(m_rotationCoroutine);

        EventManager.TriggerEvent(Event.CAMERA_LOCK_EVENT, null);

        m_dragYaw = Mathf.Clamp(m_dragYaw + input.x, -maximumRotation, maximumRotation);
        m_dragPitch = Mathf.Clamp(m_dragPitch + input.y, -maximumRotation - baseTilt, maximumRotation - baseTilt);

        Quaternion yawRot = Quaternion.AngleAxis(m_dragYaw, Vector3.up);
        Quaternion pitchRot = Quaternion.AngleAxis(m_dragPitch, Vector3.right);

        transform.rotation = yawRot * pitchRot * m_restRotation;
    }

    public void Return()
    {
        m_originRotation = transform.rotation;
        m_rotationStart = Time.time;

        if (m_rotationCoroutine != null) 
            StopCoroutine(m_rotationCoroutine);

        m_rotationCoroutine = StartCoroutine(_RotateTowards(returnDelay));
    }

}
