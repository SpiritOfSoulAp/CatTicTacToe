using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    [SerializeField] private Text turnStatusText;
    [SerializeField] private Button closeButton;

    [Header("UI Popup")]
    [SerializeField] private GameObject resultPopup;
    [SerializeField] private Text resultText;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button backToHomeButton;

    private void OnEnable()
    {
        closeButton.onClick.AddListener(OnBackToHome);
    }

    public void SetTurnStatus(string text)
    {
        turnStatusText.text = text;
    }

    public void ShowResult(string message)
    {
        resultText.text = message;
        resultPopup.SetActive(true);
    }

    private void OnPlayAgain()
    {
        resultPopup.SetActive(false);
        GameBoard.Instance.StartGame();
    }

    private void OnBackToHome()
    {
        SceneLoader.Instance.LoadScene(SceneType.Home);
    }

    public void RegisterResultButtons(System.Action onPlayAgain, System.Action onBackToHome)
    {
        playAgainButton.onClick.RemoveAllListeners();
        backToHomeButton.onClick.RemoveAllListeners();

        playAgainButton.onClick.AddListener(() =>
        {
            onPlayAgain?.Invoke();  
            OnPlayAgain();         
        });
        backToHomeButton.onClick.AddListener(() => onBackToHome?.Invoke());
    }

    private void OnDisable()
    {
        closeButton.onClick.RemoveAllListeners();
        playAgainButton.onClick.RemoveAllListeners();
        backToHomeButton.onClick.RemoveAllListeners();
    }
}
