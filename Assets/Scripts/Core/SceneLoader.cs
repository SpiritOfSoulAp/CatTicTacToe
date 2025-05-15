using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[SerializeField]
public enum SceneType
{
    Init,
    Home,
    Game
}

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

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

    private static Dictionary<SceneType, string> sceneNames = new Dictionary<SceneType, string>
    {
        { SceneType.Init, "Init" },
        { SceneType.Home, "Home" },
        { SceneType.Game, "Game" }
    };

    public void LoadScene(SceneType scene)
    {
        if (sceneNames.TryGetValue(scene, out string sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene name not found for: " + scene);
        }
    }

    public void ReloadCurrentScene()
    {
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game called");
        Application.Quit();
    }
}
