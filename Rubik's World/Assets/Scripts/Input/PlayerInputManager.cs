using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [Header("Input References")]
    [SerializeField] private InputActionReference _moveAction;

    private Pawn k_pawn;

    private bool m_pawnIsLocked = false;
    private bool m_cameraIsLocked = false;

    private void Awake()
    {
        k_pawn = GetComponent<Pawn>();

        if(k_pawn == null )
        {
            throw new MissingComponentException("The player is missing its pawn component!");
        }

        _moveAction.action.performed += OnMovePerformed;
        _moveAction.action.canceled += OnMoveCanceled;

        EventManager.StartListening(Event.CAMERA_ANIMATION_EVENT, OnCameraAnimationEvent);
        EventManager.StartListening(Event.PAWN_ANIMATION_EVENT, OnPawnAnimationEvent);
    }

    private void OnDestroy()
    {
        _moveAction.action.performed -= OnMovePerformed;
        _moveAction.action.canceled -= OnMoveCanceled;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (m_cameraIsLocked || m_pawnIsLocked)
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
    private void OnCameraAnimationEvent(System.Collections.Generic.Dictionary<string, object> message)
    {
        m_cameraIsLocked = !m_cameraIsLocked;
    }

    private void OnPawnAnimationEvent(System.Collections.Generic.Dictionary<string, object> message)
    {
        m_pawnIsLocked = !m_pawnIsLocked;
    }
}
