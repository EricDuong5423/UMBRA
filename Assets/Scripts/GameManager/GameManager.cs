using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // ── Singleton ───────────────────────────────────────────
    public static GameManager Instance { get; private set; }

    // ── Game State ───────────────────────────────────────────
    public enum GameState
    {
        Tutorial,
        Playing,
        Paused,
        GameOver,
        Victory
    };
    
    public GameState CurrentState { get; private set; }
    
    public event Action<GameState> OnGameStateChanged; 
    
    // ── Run data ───────────────────────────────────────────
    public int CurrentFloor { get; private set; } = 0;
    public float RunElapsedTime { get; private set; } = 0;
    
    // ── Scene Names ─────────────────────────────────────────
    [Header("Scene Names")]
    [SerializeField] private string tutorialSceneName = "Tutorial-Map";
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private string dungeonSceneName  = "Dungeon";
    
    // ── Private variables ─────────────────────────────────────────
    private bool isRunning = false;
    private bool isInTutorial = false;

    // ── Lifecycle ────────────────────────────────────────────
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

    private void Update()
    {
        if(isRunning && CurrentState == GameState.Playing) 
            RunElapsedTime += Time.deltaTime;
    }
    
    // ── State Control ────────────────────────────────────────
    public void SetState(GameState newState)
    {
        CurrentState = newState;
        OnGameStateChanged?.Invoke(newState);

        switch (newState)
        {
            case GameState.Paused:   Time.timeScale = 0f; break;
            case GameState.GameOver: Time.timeScale = 0f; isRunning = false; break;
            case GameState.Victory:  Time.timeScale = 0f; isRunning = false; break;
            default:                 Time.timeScale = 1f; break;
        }
    }

    public void PauseGame()
    {
        if (CurrentState == GameState.Playing
            || CurrentState == GameState.Tutorial)
        {
            SetState(GameState.Paused);
        }
    }

    public void ResumeGame()
    {
        if (CurrentState == GameState.Paused)
        {
            SetState(isInTutorial ? GameState.Tutorial : GameState.Playing);
        }
    }
    
    // ── Scene / Run Control ─────────────────────────────────────────

    public void StartTutorial()
    {
        isInTutorial = true;
        ResetRunData();
        SetState(GameState.Tutorial);
        SceneManager.LoadScene(tutorialSceneName);
    }

    public void StartNewRun()
    {
        isInTutorial = false;
        ResetRunData();
        SetState(GameState.Playing);
        isRunning = true;
        CurrentFloor = 1;
        SceneManager.LoadScene(dungeonSceneName);
    }

    public void GoNextFloor()
    {
        EnemyManager.Instance?.ReturnAllEnemies();
        CurrentFloor++;
        SetState(GameState.Playing);
        SceneManager.LoadScene(dungeonSceneName);
    }
    
    public void TriggerGameOver()
    {
        SetState(GameState.GameOver);
    }
    public void TriggerVictory()
    {
        SetState(GameState.Victory);
    }
    
    public void ReturnToMainMenu()
    {
        ResetRunData();
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
    
    // ── Helpers ──────────────────────────────────────────────
    private void ResetRunData()
    {
        CurrentFloor = 0;
        RunElapsedTime = 0f;
        isRunning = false;
    }
}
