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

        if (animator != null)
            animator.SetTrigger("Start");

        yield return new WaitForSeconds(1f);

        faderPanel.SetActive(false);
    }
}
