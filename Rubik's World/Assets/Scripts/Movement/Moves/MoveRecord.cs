using UnityEngine;

public readonly struct MoveRecord
{
    public readonly Direction Direction;
    public readonly Tile FromTile;
    public readonly Tile ToTile;
    public readonly float Timestamp;

    public MoveRecord(Direction direction, Tile fromTile, Tile toTile)
    {
        Direction = direction;
        FromTile = fromTile;
        ToTile = toTile;
        Timestamp = Time.time;
    }
}
