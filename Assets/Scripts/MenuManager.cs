using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    void Start()
    {
        CurrentLevel.ResetLevels();
    }

    public void Play()
    {
        // 1. Destruir todos los objetos persistentes
        DestroyAllPersistentObjects();

        // 2. Reiniciar el nivel (opcional, depende de tu flujo)
        CurrentLevel.ResetLevels();

        // 3. Aumentar nivel (si tu sistema requiere empezar desde level 1)
        CurrentLevel.IncreaseLevel();

        // 4. Cargar la primera escena
        SceneManager.LoadScene("LVL1");
    }

    private void DestroyAllPersistentObjects()
    {
        // Destruir objetos con CurrentGameData
        var gameDataObjects = FindObjectsOfType<CurrentGameData>();
        foreach (var obj in gameDataObjects)
        {
            if (obj != null && obj.gameObject != null)
            {
                Destroy(obj.gameObject);
            }
        }

        // Destruir objetos con Persister
        var persisters = FindObjectsOfType<Persister>();
        foreach (var p in persisters)
        {
            if (p != null && p.gameObject != null)
            {
                Destroy(p.gameObject);
            }
        }

        Debug.Log($"Se destruyeron {gameDataObjects.Length + persisters.Length} objetos persistentes");
    }

    public void ReturnToMenu()
    {

        SceneManager.LoadScene("MainMenu");

    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}