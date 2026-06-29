using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TitleMenuController : MonoBehaviour
{
    private UIDocument document;

    // TODO Add safety checks
    void Awake()
    {
        document = GetComponent<UIDocument>();
        document.rootVisualElement.Q<Button>("start-button").clicked += OnStartClicked;
    }

    private void OnStartClicked()
    {
        EventManager.TriggerEvent(Event.LEVEL_LOAD_EVENT, new Dictionary<string, object>
        {
            { "sceneName", LevelListSO.Instance.levels[0] }
        });
    }
}