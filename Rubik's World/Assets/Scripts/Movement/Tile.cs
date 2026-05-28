using System;
using System.Collections.Generic;
using UnityEngine;

public enum State { FREE, OCCUPIED, LOCKED, MARKED }

public class Tile : MonoBehaviour
{
    [SerializeField] private State state;
    [SerializeField] private Transform position;

    [SerializeField] private Tile up;
    [SerializeField] private Tile down;
    [SerializeField] private Tile left;
    [SerializeField] private Tile right;

    public void SetState(State newState) { state = newState; }

    public State GetState() => state;

    public Vector3 GetPosition() => position.position;

    public Tile GetTile(Direction direction)
    {
        switch (direction)
        {
            case Direction.UP: return up;
            case Direction.DOWN: return down;
            case Direction.RIGHT: return right;
            case Direction.LEFT: return left;
            default:
                break;
        }

        throw new NotSupportedException("The direction was not recognized.");
    }
}
