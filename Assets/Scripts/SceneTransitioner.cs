using UnityEngine;

public class SceneTransitioner : MonoBehaviour
{
    public static SceneTransitioner Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

    }



    public void LoadScene()
    {
     
        string targetScene = GetSceneByLevel(CurrentLevel.Level);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeScene(targetScene);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(targetScene);
        }
    }

    private string GetSceneByLevel(int level)
    {
        switch (level)
        {
            case 0: return "MainMenu";
            case 1: return "LVL1";
            case 2: return "LVL2";
            case 3: return "LVL3";
            case 4: return "Credits";
            default:
                Debug.LogWarning($"Nivel {level} no configurado, cargando MainMenu");
                return "MainMenu";
        }
    }
}