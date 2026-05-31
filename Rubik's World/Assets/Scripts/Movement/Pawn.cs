using UnityEngine;

public class Pawn : MonoBehaviour
{
    [SerializeField] private Tile _currentTile;

    private void Start()
    {
        MoveTo(_currentTile);
    }

    public void Move(Direction direction)
    {
        Tile _nextTile = _currentTile.GetAdjacentTile(direction);

        if (_nextTile == null)
        {
            return;
        }

        MoveTo(_nextTile);

        _currentTile = _nextTile;
    }

    private void MoveTo(Tile tile)
    {
        Transform tileTransform = tile.GetPosition();

        transform.position = tileTransform.position;
        transform.rotation = tileTransform.rotation;

        tile.SetVisited();
    }
}
