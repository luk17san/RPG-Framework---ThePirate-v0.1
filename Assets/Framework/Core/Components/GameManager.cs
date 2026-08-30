using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState State { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        State = GameState.Loading;
    }

    private void Start()
    {
        SetState(GameState.Playing);
    }

    public void SetState(GameState newState)
    {
        if (State == newState)
            return;

        State = newState;

        EventBus.Publish(new GameStateChangedEvent(newState));
    }

    public void PauseGame()
    {
        if (State != GameState.Playing)
            return;

        SetState(GameState.Paused);

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        if (State != GameState.Paused)
            return;

        Time.timeScale = 1f;

        SetState(GameState.Playing);
    }
}