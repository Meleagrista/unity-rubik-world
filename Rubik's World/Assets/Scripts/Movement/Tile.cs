using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private bool isVisited = false;
    [SerializeField] private Transform position;

    [Header("Neighbors")]
    [SerializeField] private Tile forward;
    [SerializeField] private Tile backward;
    [SerializeField] private Tile left;
    [SerializeField] private Tile right;

    [Header("Feedback")]
    [SerializeField] private Material visitedMaterial;

    public void SetVisited() { isVisited = true; }

    public bool GetVisited() => isVisited;

    public Transform GetPosition() => position;


    /* PAWN FUNCTIONS */

    public Tile GetNextTile(Vector3 pawnDirection)
    {
        Direction tileDirection = DirectionExtensions.From3DVectors(transform.forward, transform.up, pawnDirection);

        switch (tileDirection)
        {
            case Direction.FORWARD:
                return forward;
            case Direction.BACKWARD:
                return backward;
            case Direction.LEFT:
                return left;
            case Direction.RIGHT:
                return right;
            default:
                throw new NotImplementedException("Switch statemente cannot process unknonw direction.");
        }
    }

    /* CUBE FUNCTIONS */

    [ContextMenu("Detect")]
    public void DetectAdjacentTiles()
    {
        forward = null;
        backward = null;
        right = null;
        left = null;

        var directions = Enum.GetValues(typeof(Direction));
        foreach (Direction direction in directions)
        {
            DetectAdjacentTile(direction);
        }
    }

    private void DetectAdjacentTile(Direction direction)
    {
        RaycastHit hit;

        Ray ray;
        Color color = Color.white;

        switch (direction)
        {
            case Direction.FORWARD: 
                ray = new Ray(transform.position, transform.forward);
                color = Color.red;
                break;
            case Direction.BACKWARD:
                ray = new Ray(transform.position, -transform.forward);
                break;
            case Direction.RIGHT:
                ray = new Ray(transform.position, transform.right);
                break;
            case Direction.LEFT:
                ray = new Ray(transform.position, -transform.right);
                color = Color.blue;
                break;
            default:
                throw new NotSupportedException("The direction was not recognized.");
        }

        Debug.DrawRay(ray.origin, ray.direction * 2, color, 1);

        if (Physics.Raycast(ray, out hit, 2))
        {
            Tile tile = hit.collider.GetComponentInParent<Tile>();

            switch (direction)
            {
                case Direction.FORWARD:
                    forward = tile;
                    break;
                case Direction.BACKWARD:
                    backward = tile;
                    break;
                case Direction.RIGHT:
                    right = tile;
                    break;
                case Direction.LEFT:
                    left = tile;
                    break;
                default:
                    throw new NotSupportedException("The direction was not recognized.");
            }
        }
    }

    /* PROVISIONAL FUNCTIONS */

    public void UpdateTile()
    {
        if (isVisited)
        {
            MeshRenderer mesh = GetComponentInChildren<MeshRenderer>();

            if (mesh)
            {
                mesh.material = visitedMaterial;
            }  
        }
    }
}
