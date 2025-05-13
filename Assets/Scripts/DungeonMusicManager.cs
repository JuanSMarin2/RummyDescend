using UnityEngine;

public class DungeonMusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip dungeonMusic;



    public static DungeonMusicManager Instance { get; private set; }

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
    private void Start()
    {
        // Asegurarse de que tenemos un AudioSource
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configurar y reproducir la música del dungeon
        audioSource.clip = dungeonMusic;
        audioSource.loop = true;
        audioSource.Play();

        // Verificar si ya es el jefe final al iniciar
        CheckForFinalBoss();
    }

    private void Update()
    {
        // Verificar constantemente si se convierte en jefe final
        CheckForFinalBoss();
    }

    private void CheckForFinalBoss()
    {
        if (GameManager.Instance.isFinalBoss)
        {
            // Detener la música y destruir el objeto
            if (audioSource != null)
            {
                audioSource.Stop();
            }
            Destroy(gameObject);
        }
    }
}