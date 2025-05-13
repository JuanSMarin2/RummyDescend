using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SceneRestorer : MonoBehaviour
{
    [SerializeField] private GameObject player;






    void Start()
    {
        if (GameData.Instance != null)
        {
            player.transform.position = GameData.Instance.lastPlayerPosition;

            foreach (string defeatedID in GameData.Instance.defeatedEnemies)
            {
                GameObject enemy = GameObject.Find(defeatedID);
                if (enemy != null)
                {
                    EnemyMovement enemyScript = enemy.GetComponent<EnemyMovement>();
                    if (enemyScript != null)
                        enemy.SetActive(false);
                }
            }
        }

        foreach (GameObject door in GameObject.FindGameObjectsWithTag("Door"))
        {
            if (GameData.Instance.IsDoorOpened(door.name))
            {
                door.SetActive(false);
            }
        }

      

    }

    
}
