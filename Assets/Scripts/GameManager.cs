using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject faderPanel;

    [SerializeField] public bool hasKey;

    

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);


        if (faderPanel != null)
            faderPanel.SetActive(true);


    }

    private void HideDefeatedEnemies()
    {
        if (GameData.Instance == null) return;

        // Buscar todos los enemigos en la escena
        EnemyMovement[] enemiesInScene = FindObjectsOfType<EnemyMovement>();

        // Recorrer todos los enemigos y desactivar los que estén en la lista de derrotados
        foreach (EnemyMovement enemy in enemiesInScene)
        {
            if (GameData.Instance.defeatedEnemies.Contains(enemy.gameObject.name))
            {
                enemy.gameObject.SetActive(false); // Desactivar enemigos derrotados
            }
        }
    }


    public void ChangeScene(string sceneName)
    {

        StartCoroutine(LoadLevel(sceneName));
    }

    private IEnumerator LoadLevel(string sceneName)
    {
        faderPanel.SetActive(true);

        if (animator != null)
            animator.SetTrigger("End");

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);

        yield return null; // Espera un frame por seguridad

        // Aquí ocultamos los enemigos derrotados
        HideDefeatedEnemies();

        if (animator != null)
            animator.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        faderPanel.SetActive(false);
    }
}
