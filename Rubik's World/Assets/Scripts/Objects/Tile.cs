using System;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private int isVisited = 0;
    [SerializeField] private Transform position;

    [Header("Neighbors")]
    [SerializeField] private Tile forward;
    [SerializeField] private Tile backward;
    [SerializeField] private Tile left;
    [SerializeField] private Tile right;

    private MeshRenderer k_meshRenderer;
    private Material k_originalMaterial;

    [Header("Feedback")]
    [SerializeField] private Material visitedMaterial;


    private void Awake()
    {
        k_meshRenderer = GetComponentInChildren<MeshRenderer>();
        k_originalMaterial = k_meshRenderer.material;
    }

    public void SetVisited(bool isForward = true) 
    { 
        isVisited = isForward ? isVisited + 1 : isVisited - 1;

        if (isVisited < 0)
            throw new ArgumentOutOfRangeException("A tile cannot be visited less than zero times.");
    }

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

    /* PROVISIONAL FUNCTIONS */

    public void UpdateTile()
    {
        if (isVisited > 0)
        {
            k_meshRenderer.material = visitedMaterial;
        }
        else
        {
            k_meshRenderer.material = k_originalMaterial;
        }
    }
}
