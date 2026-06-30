using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [Header("Input References")]
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _undoAction;

    private PlayerController k_pawn;

    private bool m_pawnIsLocked = false;
    private bool m_cameraIsLocked = false;
    private bool m_gameEnded = false;

    private void Awake()
    {
        k_pawn = GetComponent<PlayerController>();

        if(k_pawn == null )
        {
            throw new MissingComponentException("The player is missing its pawn component!");
        }

        _moveAction.action.performed += OnMovePerformed;
        _moveAction.action.canceled += OnMoveCanceled;

        _undoAction.action.performed += OnUndoPerformed;
        _undoAction.action.canceled += OnUndoCanceled;

        EventManager.StartListening(Event.CAMERA_LOCK_EVENT, OnCameraLockEvent);
        EventManager.StartListening(Event.CAMERA_UNLOCK_EVENT, OnCameraUnlockEvent);
        EventManager.StartListening(Event.PAWN_ANIMATION_EVENT, OnPawnAnimationEvent);
        EventManager.StartListening(Event.GAME_WIN_EVENT, OnGameEnded);
        EventManager.StartListening(Event.GAME_LOSE_EVENT, OnGameEnded);
        EventManager.StartListening(Event.GAME_STARTED_EVENT, OnGameStarted);
    }

    private void OnDestroy()
    {
        _moveAction.action.performed -= OnMovePerformed;
        _moveAction.action.canceled -= OnMoveCanceled;

        _undoAction.action.performed -= OnUndoPerformed;
        _undoAction.action.canceled -= OnUndoCanceled;

        EventManager.StopListening(Event.CAMERA_LOCK_EVENT, OnCameraLockEvent);
        EventManager.StopListening(Event.CAMERA_UNLOCK_EVENT, OnCameraUnlockEvent);
        EventManager.StopListening(Event.PAWN_ANIMATION_EVENT, OnPawnAnimationEvent);
        EventManager.StopListening(Event.GAME_WIN_EVENT, OnGameEnded);
        EventManager.StopListening(Event.GAME_LOSE_EVENT, OnGameEnded);
        EventManager.StopListening(Event.GAME_STARTED_EVENT, OnGameStarted);
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (m_gameEnded || m_cameraIsLocked || m_pawnIsLocked)
        {
            return;
        }

        Vector2 input = context.ReadValue<Vector2>();

        Direction direction = DirectionExtensions.From2DVector(input);

        k_pawn.Move(direction);

    }
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {

    }

    private void OnUndoPerformed(InputAction.CallbackContext context)
    {
        if (m_gameEnded || m_cameraIsLocked || m_pawnIsLocked)
        {
            return;
        }

        k_pawn.Undo();
    }

    private void OnUndoCanceled(InputAction.CallbackContext context)
    {

    }

    private void OnCameraLockEvent(System.Collections.Generic.Dictionary<string, object> message)
    {
        m_cameraIsLocked = true;
    }
    private void OnCameraUnlockEvent(System.Collections.Generic.Dictionary<string, object> message)
    {
        m_cameraIsLocked = false;
    }

    private void OnPawnAnimationEvent(System.Collections.Generic.Dictionary<string, object> message)
    {
        m_pawnIsLocked = !m_pawnIsLocked;
    }

    private void OnGameEnded(System.Collections.Generic.Dictionary<string, object> message)
    {
        m_gameEnded = true;
    }

    private void OnGameStarted(System.Collections.Generic.Dictionary<string, object> message)
    {
        m_gameEnded = false;
    }
}
