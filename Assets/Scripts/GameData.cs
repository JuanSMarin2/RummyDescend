using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance { get; private set; }

    public Vector2 lastPlayerPosition;
    public string nextSceneName;

    // Enemigos derrotados
    public List<string> defeatedEnemies = new List<string>();

    // Puertas abiertas
    public List<string> openedDoors = new List<string>();

    // Nuevo: ID del �ltimo enemigo que activ� el combate
    public string lastEnemyID;

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

    // A�adir enemigo derrotado
    public void RegisterDefeatedEnemy(string enemyID)
    {
        if (!defeatedEnemies.Contains(enemyID))
        {
            defeatedEnemies.Add(enemyID);
        }
    }

    // Comprobar si un enemigo ya fue derrotado
    public bool IsEnemyDefeated(string enemyID)
    {
        return defeatedEnemies.Contains(enemyID);
    }

    // A�adir puerta abierta
    public void RegisterOpenedDoor(string doorID)
    {
        if (!openedDoors.Contains(doorID))
            openedDoors.Add(doorID);
    }

    // Comprobar si una puerta ya fue abierta
    public bool IsDoorOpened(string doorID)
    {
        return openedDoors.Contains(doorID);
    }
}