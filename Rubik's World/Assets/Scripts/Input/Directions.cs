using System;
using Unity.VisualScripting;
using UnityEngine;

public enum Direction
{
    FORWARD,
    BACKWARD,
    RIGHT,
    LEFT,
    INVALID
}

public static class DirectionExtensions
{
    // static private float k_vectorSimilarityThreshold = 4f;

    public static Direction From2DVector(Vector2 input)
    {
        if (input.x == 0)
        {
            return input.y > 0 ? Direction.FORWARD : Direction.BACKWARD;
        }
        else
        {
            return input.x > 0 ? Direction.RIGHT : Direction.LEFT;
        }

        throw new NotSupportedException("The direction was not recognized.");
    }

    // The refence is the foward vector of the object, while input is vector we want to know the direction of respective to the object.
    public static Direction From3DVectors(Vector3 referenceForward, Vector3 referenceUp, Vector3 input)
    {
        if (referenceForward == input)
        {
            return Direction.FORWARD;
        }

        if (referenceForward == -input)
        {
            return Direction.BACKWARD;
        }

        Vector3 referenceRight = Vector3.Cross(referenceForward, referenceUp);

        if (referenceRight == input)
        {
            return Direction.LEFT;
        }

        if (referenceRight == -input)
        {
            return Direction.RIGHT;
        }

        Debug.LogError("The direction was not recognized!");

        return Direction.INVALID;
    }
}