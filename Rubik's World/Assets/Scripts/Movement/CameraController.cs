using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 2.0f;
    [SerializeField] private float returnDelay = 0.0f;
    [SerializeField] private float baseTilt = 40f;
    [SerializeField] private float maximumRotation = 180;

    private Quaternion m_baseRotation;
    private Quaternion m_originRotation;
    private Quaternion m_targetRotation;
    private Quaternion m_restRotation;
    private Quaternion m_faceRotation;      // pawn orientation without tilt

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
        m_restRotation   = transform.rotation;
        m_faceRotation   = Quaternion.identity;
    }

    private IEnumerator _RotateTowards(float delayInSeconds = 0.0f)
    {
        if (delayInSeconds > 0f)
            yield return new WaitForSeconds(delayInSeconds);

        // Start timing after the delay, not before.
        m_rotationStart = Time.time;    

        if (m_originRotation != m_targetRotation)
        {
            while (transform.rotation != m_targetRotation)
            {
                float t = (Time.time - m_rotationStart) * rotationSpeed;
                transform.rotation = Quaternion.Slerp(m_originRotation, m_targetRotation, t);
                yield return null;
            }

            m_originRotation = transform.rotation;
            m_rotationStart = -1.0f;
        }

        // One unlock per coroutine run, matches the one lock fired by the caller.
        EventManager.TriggerEvent(Event.CAMERA_UNLOCK_EVENT, null);
        m_rotationCoroutine = null;
    }

    public void RotateTowards(Quaternion target)
    {
        m_originRotation = transform.rotation;
        m_targetRotation = target * m_baseRotation;
        m_faceRotation   = target;
        m_rotationStart  = Time.time;

        if (m_rotationCoroutine != null)
            StopCoroutine(m_rotationCoroutine);

        EventManager.TriggerEvent(Event.CAMERA_LOCK_EVENT, null);
        m_rotationCoroutine = StartCoroutine(_RotateTowards());
    }

    public void Free()
    {
        m_restRotation = m_targetRotation;
        m_dragYaw = 0f;
        m_dragPitch = 0f;
    }

    public void Rotate(Vector2 input)
    {
        if (m_rotationCoroutine != null)
            StopCoroutine(m_rotationCoroutine);

        m_dragYaw   = Mathf.Clamp(m_dragYaw   + input.x, -maximumRotation, maximumRotation);
        m_dragPitch = Mathf.Clamp(m_dragPitch  + input.y, -maximumRotation - baseTilt, maximumRotation - baseTilt);

        // Axes from face orientation (no tilt) so yaw stays flat to the face on all cube sides.
        Vector3 yawAxis   = m_faceRotation * Vector3.up;
        Vector3 pitchAxis = m_faceRotation * Vector3.right;

        transform.rotation = Quaternion.AngleAxis(m_dragYaw, yawAxis)
                           * Quaternion.AngleAxis(m_dragPitch, pitchAxis)
                           * m_restRotation;
    }

    public void Return()
    {
        m_originRotation = transform.rotation;  // snapshot where we are now
        m_rotationStart = -1f;                  // will be set after the delay in the coroutine

        if (m_rotationCoroutine != null)
            StopCoroutine(m_rotationCoroutine);

        m_rotationCoroutine = StartCoroutine(_RotateTowards(returnDelay));
    }

}
