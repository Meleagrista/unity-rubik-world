using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    [Header("Input References")]
    [SerializeField] private InputActionReference _moveAction;

    const uint k_inputQueueSize = 15;

    Queue m_inputQueue = new Queue();

    private void Awake()
    {
        _moveAction.action.performed += OnMovePerformed;
        _moveAction.action.canceled += OnMoveCanceled;
    }

    private void OnDestroy()
    {
        _moveAction.action.performed -= OnMovePerformed;
        _moveAction.action.canceled -= OnMoveCanceled;
    }

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(0, 0, Screen.width, Screen.height));
        GUILayout.Label(string.Join("\n", m_inputQueue.ToArray()));
        GUILayout.EndArea();
    }

    private void Log(Vector2 input)
    {
        Direction direction = DirectionExtensions.From2DVector(input);

        m_inputQueue.Enqueue(direction.ToString());

        while (m_inputQueue.Count > k_inputQueueSize)
        {
            m_inputQueue.Dequeue();
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Log(context.ReadValue<Vector2>());
    }
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {

    }
}
