using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraInputManager : MonoBehaviour
{
    [Header("Input References")]
    [SerializeField] private InputActionReference _deltaAction;
    [SerializeField] private InputActionReference _lockAction;

    private CameraController k_cameraController;

    private bool m_isDragging = false;

    private void Awake()
    {
        k_cameraController = GetComponent<CameraController>();

        if(k_cameraController == null )
        {
            throw new MissingComponentException("The camera is missing its camera component!");
        }

        _deltaAction.action.performed += OnDelta;
        _lockAction.action.started += OnLock;
        _lockAction.action.canceled += OnLock;
    }

    private void OnDestroy()
    {
        _deltaAction.action.performed -= OnDelta;
        _lockAction.action.started -= OnLock;
        _lockAction.action.canceled -= OnLock;
    }

    [SerializeField]
    // TODO Clamp this shit.
    private float mouseSentitivity = 0.01f;

    private void OnLock(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            m_isDragging = true;
            k_cameraController.Free();
        }

        if (context.canceled)
        {
            m_isDragging = false;
            k_cameraController.Return();
        }
    }

    private void OnDelta(InputAction.CallbackContext context)
    {
        if (!m_isDragging)
            return;

        Vector2 input = context.ReadValue<Vector2>();

        input *= 0.05f;
        input *= mouseSentitivity;

        k_cameraController.Rotate(input);
    }
}
