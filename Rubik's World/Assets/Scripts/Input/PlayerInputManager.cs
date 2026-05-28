using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [Header("Input References")]
    [SerializeField] private InputActionReference _moveAction;

    private Pawn _pawn;

    private void Awake()
    {
        _pawn = GetComponent<Pawn>();

        if( _pawn == null )
        {
            throw new MissingComponentException("The player is missing its pawn component!");
        }

        _moveAction.action.performed += OnMovePerformed;
        _moveAction.action.canceled += OnMoveCanceled;
    }

    private void OnDestroy()
    {
        _moveAction.action.performed -= OnMovePerformed;
        _moveAction.action.canceled -= OnMoveCanceled;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        Direction direction = DirectionExtensions.From2DVector(input);

        _pawn.Move(direction);

    }
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {

    }
}
