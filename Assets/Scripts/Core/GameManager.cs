using UnityEngine;
using System.Collections;

public enum GameMode { SinglePlayer, TwoPlayer }
public enum Player { None, Player1, Player2 }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static event System.Action OnGameOver;
    public enum GameState { Init, Home, InGame, GameOver }

    [SerializeField] private float delayBeforeLoad = 2f;  

    public GameState CurrentState;
    public GameMode CurrentMode { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    private void Start()
    {
        CurrentState = GameState.Init;
        StartCoroutine(LoadHomeAfterDelay());
        
    }
   
    private IEnumerator LoadHomeAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeLoad); 
        InitializeGame();
    }

    public void InitializeGame()
    {
        SceneLoader.Instance.LoadScene(SceneType.Home);
        CurrentState = GameState.Home;
    }

    public void StartGame(GameMode mode)
    {
        CurrentMode = mode;

        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance is NULL!");
            return;
        }
  
        if (mode == GameMode.SinglePlayer)
        {
            SceneLoader.Instance.LoadScene(SceneType.Game); 
        }
        else if (mode == GameMode.TwoPlayer)
        {
            SceneLoader.Instance.LoadScene(SceneType.Game); 
        }
        CurrentState = GameState.InGame;
    }

    public void EndGame(Player winner)
    {
        OnGameOver?.Invoke();
        CurrentState = GameState.GameOver;
        SceneLoader.Instance.LoadScene(SceneType.Home); 
    }
}

