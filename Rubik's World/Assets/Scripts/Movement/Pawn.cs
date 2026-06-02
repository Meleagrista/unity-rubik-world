using System;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    [SerializeField] private Tile _currentTile;

    private Camera k_camera;

    private void Start()
    {
        k_camera = FindFirstObjectByType<Camera>();

        if(k_camera == null)
        {
            Debug.LogError("No camera was found");
            throw new MissingComponentException();
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
        Transform tileTransform = tile.GetPosition();
        
        transform.position = tileTransform.position;
    }

    private void RotatePawn(Tile tile, Direction direction)
    {
        Vector3 currentNormal = _currentTile.transform.up;
        Vector3 nextNormal = tile.GetPosition().transform.up;

        Quaternion cornerRotation = Quaternion.FromToRotation(currentNormal, nextNormal);
        Quaternion pawnRotation = cornerRotation * transform.rotation;

        transform.rotation = pawnRotation;

        k_camera.RotateCamera(pawnRotation);
    }
}
