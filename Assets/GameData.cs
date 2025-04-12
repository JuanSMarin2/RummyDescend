using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance { get; private set; }

    public Vector2 lastPlayerPosition;
    public string nextSceneName;

    // Nueva lista para enemigos derrotados
    public List<string> defeatedEnemies = new List<string>();

    public List<string> openedDoors = new List<string>();

    public void RegisterOpenedDoor(string doorID)
    {
        if (!openedDoors.Contains(doorID))
            openedDoors.Add(doorID);
    }

    public bool IsDoorOpened(string doorID)
    {
        return openedDoors.Contains(doorID);
    }

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

    // Llamar esto cuando derrotes a un enemigo
    public void RegisterDefeatedEnemy(string enemyID)
    {
        if (!defeatedEnemies.Contains(enemyID))
        {
            defeatedEnemies.Add(enemyID);
        }
    }
}
