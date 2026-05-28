using System;
using UnityEngine;

public enum Direction
{
    UP,
    DOWN,
    RIGHT,
    LEFT
}

public static class DirectionExtensions
{
    public static Direction From2DVector(Vector2 input)
    {
        if (input.x == 0)
        {
            return input.y > 0 ? Direction.UP : Direction.DOWN;
        }
        else
        {
            return input.x > 0 ? Direction.RIGHT : Direction.LEFT;
        }

        throw new NotSupportedException("The direction was not recognized.");
    }
}