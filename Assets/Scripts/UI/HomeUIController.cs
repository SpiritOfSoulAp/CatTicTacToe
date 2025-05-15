using UnityEngine;
using UnityEngine.UI;

public class HomeUIController : MonoBehaviour
{
    [SerializeField] private Button singlePlayerButton;
    [SerializeField] private Button twoPlayerButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        singlePlayerButton.onClick.AddListener(() => StartGame(GameMode.SinglePlayer));
        twoPlayerButton.onClick.AddListener(() => StartGame(GameMode.TwoPlayer));
        exitButton.onClick.AddListener(() => SceneLoader.Instance.ExitGame());       
    }

    private void OnDisable()
    {
        singlePlayerButton.onClick.RemoveListener(() => StartGame(GameMode.SinglePlayer));
        twoPlayerButton.onClick.RemoveListener(() => StartGame(GameMode.TwoPlayer));
        exitButton.onClick.RemoveListener(() => SceneLoader.Instance.ExitGame());
    }
    
    private void StartGame(GameMode mode)
    {
        GameManager.Instance.StartGame(mode);
        SceneLoader.Instance.LoadScene(SceneType.Game);
    }
}
