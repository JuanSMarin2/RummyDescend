using System.Collections.Generic;
using UnityEngine;

public class EnemyCardSequenceManager : MonoBehaviour
{
    [SerializeField] private Transform[] enemyDropZones; // Slots de cartas enemigas (0, 1, 2)
    [SerializeField] private GameObject enemyCardPrefab; // Prefab visual de la carta enemiga

    private List<Card> enemyCurrentSequence = new List<Card>();

    public static EnemyCardSequenceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void DisplayEnemyAttack(List<Card> enemyAttack)
    {
        ClearEnemyDisplay();

        enemyCurrentSequence = new List<Card>(enemyAttack);

        for (int i = 0; i < enemyAttack.Count && i < enemyDropZones.Length; i++)
        {
            Transform targetZone = enemyDropZones[i];

            // Instanciar el prefab en la zona correspondiente
            GameObject cardGO = Instantiate(enemyCardPrefab, targetZone);
            cardGO.transform.localPosition = Vector3.zero;
            cardGO.transform.localScale = Vector3.one;

            // Configurar visualmente la carta (si tiene lógica visual)
            EnemyCardUI enemyCardUI = cardGO.GetComponent<EnemyCardUI>();
            if (enemyCardUI != null)
            {
                enemyCardUI.Setup(enemyAttack[i]); // Puedes adaptar Setup() según cómo sea tu UI
            }
        }
    }

    public void ClearEnemyDisplay()
    {
        foreach (Transform zone in enemyDropZones)
        {
            foreach (Transform child in zone)
            {
                Destroy(child.gameObject);
            }
        }

        enemyCurrentSequence.Clear();
    }

    public List<Card> GetEnemySequence()
    {
        return enemyCurrentSequence;
    }
}
