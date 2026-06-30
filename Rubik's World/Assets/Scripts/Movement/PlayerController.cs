using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PawnController
{
    private CameraController k_camera;

    private readonly MoveLog m_moveLog = new MoveLog();

    public IReadOnlyList<MoveRecord> MoveHistory => m_moveLog.Records;

    protected override void Start()
    {
        k_camera = FindFirstObjectByType<CameraController>();

        if (k_camera == null)
        {
            throw new MissingComponentException("The player couldn't find no camera component!");
        }

        base.Start();
    }

    public override bool Move(Direction direction)
    {
        Tile previousTile = _currentTile;

        if (!base.Move(direction))
        {
            return false;
        }

        m_moveLog.Record(new MoveRecord(direction, previousTile, _currentTile));

        NotifyMoveCountChanged();

        return true;
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

        NotifyMoveCountChanged();
    }

    private void NotifyMoveCountChanged()
    {
        EventManager.TriggerEvent(Event.PAWN_ACTION_EVENT, new Dictionary<string, object>
        {
            { "count", m_moveLog.Records.Count }
        });
    }

    protected override bool RotatePawn(Tile fromTile, Tile toTile)
    {
        if (!base.RotatePawn(fromTile, toTile))
        {
            return false;
        }

        k_camera.RotateTowards(transform.rotation);

        return true;
    }
}
