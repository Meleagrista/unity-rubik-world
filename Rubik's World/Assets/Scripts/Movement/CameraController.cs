using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 2.0f;
    [SerializeField] private float returnDelay = 2.0f;

    private Quaternion m_originRotation;
    private Quaternion m_targetRotation;

    private float m_rotationStart = -1.0f;

    private Coroutine m_rotationCoroutine = null;

    private void Start()
    {
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
        m_targetRotation = target;
        m_rotationStart = Time.time;

        m_rotationCoroutine = StartCoroutine(_RotateTowards());
    }

    [SerializeField] private float maximumRotation = 180;

    public void Rotate(Vector2 input)
    {
        if (m_rotationCoroutine != null)
        {
            StopCoroutine(m_rotationCoroutine);
        }

        // Maybe too wasteful.
        EventManager.TriggerEvent(Event.CAMERA_LOCK_EVENT, null);

        float forwardRotation = -input.y * maximumRotation;
        float sideRotation = -input.x * maximumRotation;

        transform.Rotate(new Vector3(1, 0, 0), forwardRotation);
        transform.Rotate(new Vector3(0, 0, 1), sideRotation, Space.World);

        // TODO Check whether the rotation has reached the limit.
    }

    public void Return()
    {
        m_originRotation = transform.rotation;
        m_rotationStart = Time.time;

        m_rotationCoroutine = StartCoroutine(_RotateTowards(returnDelay));
    }

}
