using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    [Header("Configuración de Enemigo")]
    private EnemyData currentEnemy;

    [Header("Turnos")]
    public bool isPlayerTurn = true;
    private bool isEnemyTurn = false;

    [Header("Otros Flags")]
    public bool canReflect;
    public bool inventoryGenerated;

    private List<Card> currentEnemyAttack;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        currentEnemy = GameData.Instance.lastEnemyData;

        if (currentEnemy == null)
        {
            Debug.LogError("No se encontró el EnemyData. Asegúrate de asignarlo desde el mapa.");
            return;
        }

        Debug.Log($"Combatiendo contra: {currentEnemy.enemyName} (vida: {currentEnemy.health})");
        Debug.Log($"Jugador empieza con: {GameManager.Instance.playerHealth} de vida");

        isPlayerTurn = true;
    }

    void Update()
    {
        if (CardInventory.Instance.cards.Count == 0 && !inventoryGenerated)
        {
            for (int i = 0; i < 7; i++)
            {
                CardInventory.Instance.AddRandomCard();
            }

            CardInventoryUI.Instance.RefreshUI();
            inventoryGenerated = true;
        }

        // Turno del enemigo: genera ataque
        if (!isPlayerTurn && isEnemyTurn)
        {
            currentEnemyAttack = GetRandomEnemyAttack();

            if (currentEnemyAttack != null)
            {
                Debug.Log($"{currentEnemy.enemyName} lanza ataque:");
                foreach (var card in currentEnemyAttack)
                {
                    Debug.Log($"{card}");
                }
            }

            isEnemyTurn = false; // Avanzamos a la fase de reflejo
        }

        // Fase post ataque enemigo: verificar si se puede reflejar
        if (!isPlayerTurn && !isEnemyTurn && !canReflect)
        {
            if (PlayerCanReflect())
            {
                canReflect = true;
                Debug.Log("Jugador puede reflejar. Esperando acción...");
            }
            else
            {
                Debug.Log("Jugador no puede reflejar. Pierde vida.");
                ApplyEnemyDamage();
                isPlayerTurn = true;
            }
        }
    }

    private List<Card> GetRandomEnemyAttack()
    {
        if (currentEnemy.attackCombinations.Count == 0)
        {
            Debug.LogWarning("El enemigo actual no tiene combinaciones configuradas.");
            return null;
        }

        int index = Random.Range(0, currentEnemy.attackCombinations.Count);
        return currentEnemy.attackCombinations[index].cards;
    }

    public void LaunchPlayerAttack()
    {
        Debug.Log("Jugador lanza ataque");

        List<Card> playerAttack = CardSequenceManager.Instance.GetCurrentSequence();

        int totalDamage = 0;
        foreach (Card card in playerAttack)
        {
            totalDamage += card.number;
            CardInventory.Instance.RemoveCard(card); // Remover del inventario
        }

        currentEnemy.health -= totalDamage;
        Debug.Log($"¡Le hiciste {totalDamage} de daño al enemigo!");

        CardSequenceManager.Instance.ClearPlayedCards(); // Quitar del tablero visual

        isPlayerTurn = false;
        isEnemyTurn = true;
    }

    public void ReflectAttackWith(Card reflectCard)
    {
        Debug.Log("Jugador ha reflejado el ataque con: " + reflectCard.number + " " + reflectCard.suit);
        canReflect = false;

        int enemyAttackSum = 0;
        foreach (Card c in currentEnemyAttack)
        {
            enemyAttackSum += c.number;
        }

        int damageToEnemy = Mathf.CeilToInt((enemyAttackSum + reflectCard.number) / 2f);
        DamageEnemy(damageToEnemy);

        CardInventory.Instance.RemoveCard(reflectCard);
        isPlayerTurn = true;
    }

    public void DamageEnemy(int damage)
    {
        currentEnemy.health -= damage;
        Debug.Log($"¡Le hiciste {damage} de daño al enemigo!");

        if (currentEnemy.health <= 0)
        {
            Debug.Log("¡El enemigo ha sido derrotado!");
            // Aquí podrías cargar siguiente escena, mostrar victoria, etc.
        }
    }

    public void SkipReflect()
    {
        if (canReflect)
        {
            Debug.Log("Jugador decidió no reflejar.");
            canReflect = false;
            ApplyEnemyDamage();
            isPlayerTurn = true;
        }
    }

    private void ApplyEnemyDamage()
    {
        int damage = currentEnemyAttack.Count;
        GameManager.Instance.playerHealth -= damage;

        Debug.Log($"¡El enemigo te ha hecho {damage} de daño!");
        Debug.Log($"Vida restante del jugador: {GameManager.Instance.playerHealth}");

        if (GameManager.Instance.playerHealth <= 0)
        {
            Debug.Log("¡El jugador ha sido derrotado!");
        }
    }

    private bool PlayerCanReflect()
    {
        if (currentEnemyAttack == null || currentEnemyAttack.Count == 0)
            return false;

        List<Card> playerCards = CardInventory.Instance.cards;
        Card lastEnemyCard = currentEnemyAttack[currentEnemyAttack.Count - 1];

        foreach (Card playerCard in playerCards)
        {
            bool validBySuit = playerCard.suit == lastEnemyCard.suit && playerCard.number == lastEnemyCard.number + 1;
            bool validByNumber = playerCard.number == lastEnemyCard.number && playerCard.suit != lastEnemyCard.suit;

            if (validBySuit || validByNumber)
            {
                return true;
            }
        }

        return false;
    }

    public List<Card> GetCurrentEnemyAttack()
    {
        return currentEnemyAttack;
    }

    public bool IsReflectAvailable()
    {
        return canReflect && currentEnemyAttack != null && currentEnemyAttack.Count > 0;
    }

    public Card GetLastEnemyCard()
    {
        return currentEnemyAttack[currentEnemyAttack.Count - 1];
    }

    public void EndEnemyTurnAfterReflect()
    {
        canReflect = false;
        isPlayerTurn = true;
        isEnemyTurn = false;
    }
}
