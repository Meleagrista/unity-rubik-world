using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 2.0f;

    private Quaternion m_originRotation;
    private Quaternion m_targetRotation;

    private float m_rotationStart = -1.0f;

    private void Start()
    {
        m_originRotation = transform.rotation;
        m_targetRotation = transform.rotation;
    }

    private void Update()
    {
        if (m_originRotation == m_targetRotation || m_rotationStart < 0)
        {
            return;
        }

        float t = (Time.time - m_rotationStart) * rotationSpeed;
        transform.rotation = Quaternion.Slerp(m_originRotation, m_targetRotation, t);

        if (transform.rotation == m_targetRotation)
        {
            m_originRotation = transform.rotation;
            m_rotationStart = -1.0f;

            EventManager.TriggerEvent(Event.CAMERA_ANIMATION_EVENT, null);
        }
    }

    public void RotateCamera(Quaternion rotation)
    {
        m_originRotation = transform.rotation;
        m_targetRotation = rotation;
        m_rotationStart = Time.time;

        EventManager.TriggerEvent(Event.CAMERA_ANIMATION_EVENT, null);
    }

}
