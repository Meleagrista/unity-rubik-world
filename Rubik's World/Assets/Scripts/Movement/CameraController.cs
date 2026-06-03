using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
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

    private IEnumerator Rotate()
    {
        if (!(m_originRotation == m_targetRotation || m_rotationStart < 0))
        {
            EventManager.TriggerEvent(Event.CAMERA_ANIMATION_EVENT, null);

            while (transform.rotation != m_targetRotation)
            {
                float t = (Time.time - m_rotationStart) * rotationSpeed;
                transform.rotation = Quaternion.Slerp(m_originRotation, m_targetRotation, t);

                yield return null;
            }

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

        StartCoroutine(Rotate());
    }

}
