using System;
using UnityEngine;

public class PawnController : MonoBehaviour
{
    [SerializeField] protected Tile _currentTile;

    protected virtual void Start()
    {
        MovePawn(_currentTile);

        _currentTile.SetVisited();
        _currentTile.UpdateTile();
    }

    public virtual bool Move(Direction direction)
    {
        Vector3 pawnDirection = DirectionToVector(direction);

        Tile nextTile = _currentTile.GetNextTile(pawnDirection);

        if (nextTile == null)
        {
            return false;
        }

        MovePawn(nextTile);
        RotatePawn(_currentTile, nextTile);

        nextTile.SetVisited();
        nextTile.UpdateTile();

        _currentTile = nextTile;

        return true;
    }

    private Vector3 DirectionToVector(Direction direction)
    {
        switch (direction)
        {
            case Direction.FORWARD:
                return transform.forward;
            case Direction.BACKWARD:
                return -transform.forward;
            case Direction.RIGHT:
                return transform.right;
            case Direction.LEFT:
                return -transform.right;
            default:
                throw new NotImplementedException("Switch statemente cannot process unknonw direction.");
        }
    }

    protected virtual void MovePawn(Tile tile)
    {
        EventManager.TriggerEvent(Event.PAWN_ANIMATION_EVENT, null);

        Transform tileTransform = tile.GetPosition();

        transform.position = tileTransform.position;

        EventManager.TriggerEvent(Event.PAWN_ANIMATION_EVENT, null);
    }

    protected virtual bool RotatePawn(Tile fromTile, Tile toTile)
    {
        Vector3 currentNormal = fromTile.transform.up;
        Vector3 nextNormal = toTile.GetPosition().transform.up;

        Quaternion cornerRotation = Quaternion.FromToRotation(currentNormal, nextNormal);
        Quaternion pawnRotation = cornerRotation * transform.rotation;

        if (transform.rotation == pawnRotation)
        {
            return false;
        }

        transform.rotation = pawnRotation;

        return true;
    }
}
