using System.Collections.Generic;
using UnityEngine;

public class ReachTileWinCondition : WinCondition
{
    [SerializeField] private Tile targetTile;

    protected override void OnTileVisited(Dictionary<string, object> message)
    {
        Tile visitedTile = (Tile)message["tile"];

        if (visitedTile == targetTile)
        {
            DeclareWin();
        }
    }
}
