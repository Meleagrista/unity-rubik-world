using System.Collections.Generic;
using UnityEngine;

public abstract class WinCondition : MonoBehaviour
{
    protected virtual void OnEnable()
    {
        EventManager.StartListening(Event.TILE_VISITED_EVENT, OnTileVisited);
        EventManager.StartListening(Event.TILE_UNVISITED_EVENT, OnTileUnvisited);
    }

    protected virtual void OnDisable()
    {
        EventManager.StopListening(Event.TILE_VISITED_EVENT, OnTileVisited);
        EventManager.StopListening(Event.TILE_UNVISITED_EVENT, OnTileUnvisited);
    }

    protected abstract void OnTileVisited(Dictionary<string, object> message);

    protected virtual void OnTileUnvisited(Dictionary<string, object> message) { }

    protected void DeclareWin()
    {
        EventManager.TriggerEvent(Event.GAME_WIN_EVENT, null);
    }
}
