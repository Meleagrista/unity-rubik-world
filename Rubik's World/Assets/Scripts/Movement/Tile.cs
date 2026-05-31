using System;
using UnityEngine;

public enum State { FREE, OCCUPIED }

public class Tile : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private State state;
    [SerializeField] private bool isVisited = false;
    [SerializeField] private Transform position;

    [Header("Neighbors")]
    [SerializeField] private Tile up;
    [SerializeField] private Tile down;
    [SerializeField] private Tile left;
    [SerializeField] private Tile right;

    [Header("Feedback")]
    [SerializeField] private Material visitedMaterial;

    // public void SetState(State newState) { state = newState; }

    public void SetVisited() 
    { 
        isVisited = true;
        UpdateTile();
    }

    // public State GetState() => state;

    public bool GetVisited() => isVisited;

    public Transform GetPosition() => position;


    /* PAWN FUNCTIONS */

    public Tile GetAdjacentTile(Direction direction)
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

    /* CUBE FUNCTIONS */

    [ContextMenu("Detect")]
    public void DetectAdjacentTiles()
    {
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

        switch (direction)
        {
            case Direction.UP: 
                ray = new Ray(transform.position, transform.forward); 
                break;
            case Direction.DOWN:
                ray = new Ray(transform.position, -transform.forward);
                break;
            case Direction.RIGHT:
                ray = new Ray(transform.position, transform.right);
                break;
            case Direction.LEFT:
                ray = new Ray(transform.position, -transform.right);
                break;
            default:
                throw new NotSupportedException("The direction was not recognized.");
        }

        Debug.DrawRay(ray.origin, ray.direction * 2, Color.red, 1);

        if (Physics.Raycast(ray, out hit, 2))
        {
            Tile tile = hit.collider.GetComponentInParent<Tile>();

            switch (direction)
            {
                case Direction.UP:
                    up = tile;
                    break;
                case Direction.DOWN:
                    down = tile;
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
