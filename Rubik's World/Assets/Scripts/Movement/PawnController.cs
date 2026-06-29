using System;
using System.Collections.Generic;
using UnityEngine;

public class PawnController : MonoBehaviour
{
    [SerializeField] private Tile _currentTile;

    private CameraController k_camera;

    private readonly MoveLog m_moveLog = new MoveLog();

    // public IReadOnlyList<MoveRecord> MoveHistory => m_moveLog.Records;

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
        RotatePawn(_currentTile, _nextTile);

        _nextTile.SetVisited();
        _nextTile.UpdateTile();

        m_moveLog.Record(new MoveRecord(direction, _currentTile, _nextTile));

        _currentTile = _nextTile;
    }

    public void Undo()
    {
        if (!m_moveLog.TryPop(out MoveRecord record))
        {
            return;
        }

        MovePawn(record.FromTile);
        RotatePawn(record.ToTile, record.FromTile);

        _currentTile = record.FromTile;

        record.ToTile.SetVisited(false);
        record.ToTile.UpdateTile();
    }

    private void MovePawn(Tile tile)
    {
        EventManager.TriggerEvent(Event.PAWN_ANIMATION_EVENT, null);

        Transform tileTransform = tile.GetPosition();
        
        transform.position = tileTransform.position;

        EventManager.TriggerEvent(Event.PAWN_ANIMATION_EVENT, null);
    }

    private void RotatePawn(Tile fromTile, Tile toTile)
    {
        Vector3 currentNormal = fromTile.transform.up;
        Vector3 nextNormal = toTile.GetPosition().transform.up;

        Quaternion cornerRotation = Quaternion.FromToRotation(currentNormal, nextNormal);
        Quaternion pawnRotation = cornerRotation * transform.rotation;

        if (transform.rotation != pawnRotation)
        {
            transform.rotation = pawnRotation;
            k_camera.RotateTowards(pawnRotation);
        }
    }
}
