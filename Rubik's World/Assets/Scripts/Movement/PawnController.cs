using System;
using UnityEngine;

public class PawnController : MonoBehaviour
{
    [SerializeField] private Tile _currentTile;

    private CameraController k_camera;

    private void Start()
    {
        k_camera = FindFirstObjectByType<CameraController>();

        if(k_camera == null)
        {
            throw new MissingComponentException("The player couldn't find no camera component!");
        }

        MovePawn(_currentTile);

        _currentTile.SetVisited();
        _currentTile.UpdateTile();
    }

    public void Move(Direction direction)
    {
        Vector3 pawnDirection = Vector3.zero;

        switch (direction)
        {
            case Direction.FORWARD:
                pawnDirection = transform.forward;
                break;
            case Direction.BACKWARD:
                pawnDirection = -transform.forward;
                break;
            case Direction.RIGHT:
                pawnDirection = transform.right;
                break;
            case Direction.LEFT:
                pawnDirection = -transform.right;
                break;
            default:
                throw new NotImplementedException("Switch statemente cannot process unknonw direction.");
        }

        Tile _nextTile = _currentTile.GetNextTile(pawnDirection);
    
        if (_nextTile == null)
        {
            return;
        }
    
        MovePawn(_nextTile);
        RotatePawn(_nextTile, direction);

        _nextTile.SetVisited();
        _nextTile.UpdateTile();

        _currentTile = _nextTile;
    }
    private void MovePawn(Tile tile)
    {
        EventManager.TriggerEvent(Event.PAWN_ANIMATION_EVENT, null);

        Transform tileTransform = tile.GetPosition();
        
        transform.position = tileTransform.position;

        EventManager.TriggerEvent(Event.PAWN_ANIMATION_EVENT, null);
    }

    private void RotatePawn(Tile tile, Direction direction)
    {
        Vector3 currentNormal = _currentTile.transform.up;
        Vector3 nextNormal = tile.GetPosition().transform.up;

        Quaternion cornerRotation = Quaternion.FromToRotation(currentNormal, nextNormal);
        Quaternion pawnRotation = cornerRotation * transform.rotation;

        if (transform.rotation != pawnRotation)
        {
            transform.rotation = pawnRotation;
            k_camera.RotateCamera(pawnRotation);
        }
    }
}
