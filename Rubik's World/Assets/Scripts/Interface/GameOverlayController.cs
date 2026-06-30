using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOverlayController : MonoBehaviour
{
    [Header("Interfaces")]
    [SerializeField] private UIDocument document;

    private Label timerLabel;
    private Label movesLabel;

    private float m_elapsedTime = 0f;
    private bool m_isTimerRunning = false;

    private void Awake()
    {
        timerLabel = document.rootVisualElement.Q<Label>("timer-label");
        movesLabel = document.rootVisualElement.Q<Label>("moves-label");

        EventManager.StartListening(Event.GAME_STARTED_EVENT, OnGameStarted);
        EventManager.StartListening(Event.PAWN_ACTION_EVENT, OnMoveCountChanged);
        EventManager.StartListening(Event.GAME_WIN_EVENT, OnGameEnded);
        EventManager.StartListening(Event.GAME_LOSE_EVENT, OnGameEnded);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(Event.GAME_STARTED_EVENT, OnGameStarted);
        EventManager.StopListening(Event.PAWN_ACTION_EVENT, OnMoveCountChanged);
        EventManager.StopListening(Event.GAME_WIN_EVENT, OnGameEnded);
        EventManager.StopListening(Event.GAME_LOSE_EVENT, OnGameEnded);
    }

    private void Start()
    {
        StartLevel();
    }

    private void StartLevel()
    {
        m_elapsedTime = 0f;
        m_isTimerRunning = true;

        UpdateTimer();
        UpdateCounter(0);
    }

    private void Update()
    {
        if (!m_isTimerRunning)
        {
            return;
        }

        m_elapsedTime += Time.deltaTime;
        UpdateTimer();
    }

    private void OnGameStarted(Dictionary<string, object> message)
    {
        StartLevel();
    }

    private void OnGameEnded(Dictionary<string, object> message)
    {
        m_isTimerRunning = false;
    }

    private void OnMoveCountChanged(Dictionary<string, object> message)
    {
        UpdateCounter((int)message["count"]);
    }

    private void UpdateTimer()
    {
        int minutes = Mathf.FloorToInt(m_elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(m_elapsedTime % 60f);

        timerLabel.text = $"{minutes:00}:{seconds:00}";
    }

    private void UpdateCounter(int count)
    {
        movesLabel.text = count.ToString();
    }
}
