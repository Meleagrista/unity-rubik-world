using Mono.Cecil.Cil;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameMenuController : MonoBehaviour
{
    [Header("Interfaces")]
    [SerializeField] private UIDocument winDocument;
    [SerializeField] private UIDocument loseDocument;

    private void Awake()
    {
        winDocument.rootVisualElement.Q<Button>("menu-button").clicked += OnMenuButtonClicked;
        loseDocument.rootVisualElement.Q<Button>("menu-button").clicked += OnMenuButtonClicked;

        EventManager.StartListening(Event.GAME_WIN_EVENT, OnGameWin);
        EventManager.StartListening(Event.GAME_LOSE_EVENT, OnGameLose);
    }

    private void OnDestroy()
    {
        if (winDocument != null && winDocument.rootVisualElement != null)
            winDocument.rootVisualElement.Q<Button>("menu-button").clicked -= OnMenuButtonClicked;

        if (loseDocument != null && loseDocument.rootVisualElement != null)
            loseDocument.rootVisualElement.Q<Button>("menu-button").clicked -= OnMenuButtonClicked;

        EventManager.StopListening(Event.GAME_WIN_EVENT, OnGameWin);
        EventManager.StopListening(Event.GAME_LOSE_EVENT, OnGameLose);
    }

    private void Start()
    {
        Hide(winDocument);
        Hide(loseDocument);
    }

    private void OnMenuButtonClicked()
    {
        EventManager.TriggerEvent(Event.LEVEL_LOAD_EVENT, new Dictionary<string, object>
        {
            { "sceneName", LevelListSO.Instance.titleScene }
        });
    }

    private void OnGameWin(Dictionary<string, object> message)
    {
        Show(winDocument);
    }

    private void OnGameLose(Dictionary<string, object> message)
    {
        Show(loseDocument);
    }

    private void Show(UIDocument document)
    {
        document.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    private void Hide(UIDocument document)
    {
        document.rootVisualElement.style.display = DisplayStyle.None;
    }
}
