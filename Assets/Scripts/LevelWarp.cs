using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelWarp : MonoBehaviour
{
    public static LevelWarp Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void InitiateLevelWarp()
    {
        // 1. Destruir objetos persistentes
        DestroyAllPersistentObjects();

        // 2. Aumentar nivel y obtener siguiente escena
        CurrentLevel.IncreaseLevel();
        string targetScene = CurrentLevel.GetNextScene();

        // 3. Cargar escena
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeScene(targetScene);
        }
        else
        {
            SceneManager.LoadScene(targetScene);
        }
    }

    private void DestroyAllPersistentObjects()
    {
        var persisters = FindObjectsOfType<Persister>();
        foreach (var p in persisters)
        {
            if (p != null && p.gameObject != null)
            {
                Destroy(p.gameObject);
            }
        }
    }
}