using System.Collections.Generic;
using UnityEngine;

public class VisitAllTilesWinCondition : WinCondition
{
    private int m_totalTiles;
    private int m_visitedTiles;

    private void Awake()
    {
        m_totalTiles = FindObjectsByType<Tile>(FindObjectsSortMode.None).Length;
    }

    protected override void OnTileVisited(Dictionary<string, object> message)
    {
        m_visitedTiles++;

        if (m_visitedTiles >= m_totalTiles)
        {
            DeclareWin();
        }
    }

    protected override void OnTileUnvisited(Dictionary<string, object> message)
    {
        m_visitedTiles--;
    }
}
