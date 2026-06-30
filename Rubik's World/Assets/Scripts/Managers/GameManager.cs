using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        EventManager.StartListening(Event.LEVEL_LOAD_EVENT, OnLoadLevel);
    }
    void OnDestroy() => EventManager.StopListening(Event.LEVEL_LOAD_EVENT, OnLoadLevel);
    
    private void OnLoadLevel(Dictionary<string, object> msg)
    {
        string sceneName = (string)msg["sceneName"];
        StartCoroutine(LoadScene(sceneName));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        yield return op;

        EventManager.TriggerEvent(Event.GAME_STARTED_EVENT, new Dictionary<string, object>
        {
            { "sceneName", sceneName }
        });
    }
}