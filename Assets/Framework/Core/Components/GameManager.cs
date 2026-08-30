using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void PauseGame()
    {
        if (IsPaused)
            return;

        IsPaused = true;

        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        if (!IsPaused)
            return;

        IsPaused = false;

        Time.timeScale = 1f;
    }
}