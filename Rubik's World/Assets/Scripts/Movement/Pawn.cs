using UnityEngine;

public class Pawn : MonoBehaviour
{
    [SerializeField] private Tile _currentTile;

    private void Start()
    {
        transform.position = _currentTile.GetPosition();
    }

    public void Move(Direction direction)
    {
        Tile _nextTile = _currentTile.GetTile(direction);

        if (_nextTile == null)
        {
            return;
        }

        transform.position = _nextTile.GetPosition();

        _currentTile = _nextTile;
    }
}
