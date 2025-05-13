using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip normalBattleMusic;
    public AudioClip finalBossMusic;

    private int lastLevel = -1;
    private bool lastIsFinalBoss = false;

    void Start()
    {
        // Asegurarse de que tenemos un AudioSource
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        UpdateMusic();
    }

    void Update()
    {
        // Verificar si hay cambios que requieran actualizar la música
        if (CurrentLevel.Level != lastLevel || GameManager.Instance.isFinalBoss != lastIsFinalBoss)
        {
            UpdateMusic();
        }
    }

    void UpdateMusic()
    {
        // Actualizar los valores de seguimiento
        lastLevel = CurrentLevel.Level;
        lastIsFinalBoss = GameManager.Instance.isFinalBoss;

        // Detener cualquier música que esté sonando
        if (CurrentLevel.Level == 1 || CurrentLevel.Level == 2)
            audioSource.Stop();



        // Reproducir música según las condiciones
        if (GameManager.Instance.isFinalBoss)
        {
            audioSource.clip = finalBossMusic;
            audioSource.Play();
        }
        else if (CurrentLevel.Level == 1 || CurrentLevel.Level == 2)
        {
            audioSource.clip = normalBattleMusic;
            audioSource.Play();
        }
    }
}