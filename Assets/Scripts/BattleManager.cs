using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; }

    [Header("Configuración de Enemigo")]
    private EnemyData currentEnemy;

    [Header("Turnos")]
    public bool isPlayerTurn = true;
    public bool isEnemyTurn = false;

    [Header("Otros Flags")]
    public bool canReflect;
    public bool inventoryGenerated;
    public bool isEnemyDefeated;
    public bool hasLens;
    private bool shieldActive = false;
    private bool swordActive = false;

    [Header("UI Elements")]
    [SerializeField] private GameObject enemyUsedText; 
    [SerializeField] private TextMeshProUGUI playerActionInTurnText;
    [SerializeField] private TextMeshProUGUI enemyActionInTurnText;
    [SerializeField] GameObject reflectText;
    [SerializeField] private GameObject enemyCardReflectZone;
    [SerializeField] private GameObject enemyCardPrefab;
    [SerializeField] private GameObject shieldImage;
    [SerializeField] private GameObject swordImage;
    [SerializeField] private GameObject dontReflectButton;
    [SerializeField] private GameObject gameOverPanel;

    private Coroutine hideEnemyCardsCoroutine;
    private Coroutine hidePlayerTextCoroutine;
    private Coroutine hideEnemyTextCoroutine;
    private Coroutine clearReflectCardCoroutine;
    private List<GameObject> displayedInventoryCards = new List<GameObject>();
    private bool playerIsDead;

    [SerializeField] private Transform enemyFullInventoryZone;
    private List<Card> currentEnemyAttack;
    private List<Card> playerAttackHistory;

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
        playerIsDead = false;

        // Verificación segura de GameData.Instance
        if (GameData.Instance == null)
        {
            Debug.LogError("GameData.Instance es nulo!");
            SceneManager.LoadScene("MainMenu");
            return;
        }

        currentEnemy = GameData.Instance.lastEnemyData;

        if (currentEnemy == null)
        {
            Debug.LogError("No se encontró el EnemyData. Asegúrate de asignarlo desde el mapa.");
            SceneManager.LoadScene("MainMenu");
            return;
        }

        // Verificación segura de GameManager.Instance
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance es nulo!");
            SceneManager.LoadScene("MainMenu");
            return;
        }

        Debug.Log($"Combatiendo contra: {currentEnemy.enemyName} (vida: {currentEnemy.health})");
        Debug.Log($"Jugador empieza con: {GameManager.Instance.playerHealth} de vida");

        isPlayerTurn = true;
        playerAttackHistory = new List<Card>();

        if (shieldImage != null) shieldImage.SetActive(false);
        if (swordImage != null) swordImage.SetActive(false);

        DisplayEnemyInventory();
    }

    void Update()
    {
        // Verificación segura de CardInventory.Instance
        if (CardInventory.Instance == null)
        {
            Debug.LogError("CardInventory.Instance es nulo!");
            return;
        }

        if (GameManager.Instance != null && GameManager.Instance.playerHealth <= 0)
        {
            playerIsDead = true;
            if (gameOverPanel != null) gameOverPanel.SetActive(true);
            Debug.Log("¡El jugador ha sido derrotado!");
        }


        if (isPlayerTurn)
        {
            // Limpiar el historial al comenzar el turno del jugador
            if (playerAttackHistory != null && playerAttackHistory.Count > 0)
            {
                ClearPlayerAttackHistory();
            }

        }

        if (!isPlayerTurn && !isEnemyTurn)
        {
            if (dontReflectButton != null) dontReflectButton.SetActive(true);
        }
        else
        {
            if (dontReflectButton != null) dontReflectButton.SetActive(false);
        }

        if (!isPlayerTurn && CardSequenceManager.Instance != null)
            CardSequenceManager.Instance.dragButton.SetActive(false);

        if (reflectText != null)
            reflectText.SetActive(canReflect);

        if (currentEnemy != null && currentEnemy.health <= 0 && !playerIsDead)
        {
            if (SceneTransitioner.Instance != null)
            {
                SceneTransitioner.Instance.LoadScene();
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            isEnemyDefeated = true;
            currentEnemy.health = currentEnemy.maxHealth;
        }

        if (CardInventory.Instance.cards.Count == 0 && !inventoryGenerated)
        {
            for (int i = 0; i < 7; i++)
            {
                CardInventory.Instance.AddRandomCard();
            }

            if (CardInventoryUI.Instance != null)
            {
                CardInventoryUI.Instance.RefreshUI();
            }
            inventoryGenerated = true;
        }

        // Turno del enemigo: genera ataque
        if (!isPlayerTurn && isEnemyTurn && !isEnemyDefeated && currentEnemy != null)
        {
            currentEnemyAttack = GetRandomEnemyAttack();

            if (currentEnemyAttack != null && currentEnemyAttack.Count > 0)
            {
                Debug.Log($"{currentEnemy.enemyName} lanza ataque:");
                foreach (var card in currentEnemyAttack)
                {
                    Debug.Log($"{card}");
                }

                if (EnemyCardSequenceManager.Instance != null)
                {
                    EnemyCardSequenceManager.Instance.DisplayEnemyAttack(currentEnemyAttack);
                }

                if (enemyUsedText != null)
                {
                    enemyUsedText.SetActive(true);
                }

                if (!PlayerCanReflect())
                {
                    if (hideEnemyCardsCoroutine != null)
                        StopCoroutine(hideEnemyCardsCoroutine);
                    hideEnemyCardsCoroutine = StartCoroutine(HideEnemyCardsAfterDelay(3f));
                }


            }

            isEnemyTurn = false;
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
                if (EnemyCanReflectAttack())
                {
                    Debug.Log("¡El enemigo refleja el ataque!");
                }
                else
                {
                    Debug.Log("Jugador no puede reflejar. Pierde vida.");
                    ApplyEnemyDamage();
                }

                if (CardSequenceManager.Instance != null)
                {
                    CardSequenceManager.Instance.dragButton.SetActive(true);
                }
                isPlayerTurn = true;
            }
        }
    }

    public EnemyData GetCurrentEnemy()
    {
        return currentEnemy;
    }

    private List<Card> GetRandomEnemyAttack()
    {
        if (currentEnemy == null || currentEnemy.attackCombinations == null || currentEnemy.attackCombinations.Count == 0)
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

        if (CardSequenceManager.Instance == null)
        {
            Debug.LogError("CardSequenceManager.Instance es nulo!");
            return;
        }

        List<Card> playerAttack = CardSequenceManager.Instance.GetCurrentSequence();
        if (playerAttack == null || playerAttack.Count == 0)
        {
            Debug.LogWarning("Intento de lanzar ataque sin cartas seleccionadas");
            return;
        }

        playerAttackHistory = new List<Card>(playerAttack);
        int totalDamage = 0;
        float damageMultiplier = 1f;

        if (swordActive)
        {
            damageMultiplier = 1.5f;
            Debug.Log("¡Espada activa! Daño multiplicado por 1.5");
            swordActive = false;
            if (swordImage != null) swordImage.SetActive(false);
        }

        if (playerAttack.Count == 3)
        {
            if (CardSequenceManager.Instance.IsStraightFlush(playerAttack))
            {
                damageMultiplier = 1.5f;
                Debug.Log("¡Escalera completa! Daño multiplicado por 1.5.");
            }
            else if (CardSequenceManager.Instance.IsThreeOfAKind(playerAttack))
            {
                damageMultiplier = 1.3f;
                Debug.Log("¡Combinación de tres! Daño multiplicado por 1.3.");
            }
        }

        foreach (Card card in playerAttack)
        {
            totalDamage += card.number;
            CardInventory.Instance.RemoveCard(card);
        }

        int finalDamage = Mathf.RoundToInt(totalDamage * damageMultiplier);
        DamageEnemy(finalDamage);
        ShowTemporaryPlayerMessage($"Hiciste {finalDamage} de daño");
        Debug.Log($"¡Le hiciste {finalDamage} de daño al enemigo!");

        CardSequenceManager.Instance.ClearPlayedCards();
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

        ShowTemporaryPlayerMessage($"Reflejaste {damageToEnemy} de daño");
        DamageEnemy(damageToEnemy);

        CardInventory.Instance.RemoveCard(reflectCard);
        isPlayerTurn = true;

        // Ocultar elementos del enemigo después de reflejar
        if (enemyUsedText != null)
        {
            enemyUsedText.SetActive(false);
        }

        // Limpiar el display de cartas enemigas
        ClearEnemyAttackImmediately();
    }

    private void ClearEnemyAttackImmediately()
    {
        EnemyCardSequenceManager.Instance.ClearEnemyDisplay();
        currentEnemyAttack = null;
    }

    public void DamageEnemy(int damage)
    {
        currentEnemy.health -= damage;
        Debug.Log($"Le hiciste {damage} de daño al enemigo!");

        if (currentEnemy.health <= 0)
        {
            Debug.Log("¡El enemigo ha sido derrotado!");
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
            CardSequenceManager.Instance.dragButton.SetActive(true);
        }
    }

    private void ApplyEnemyDamage()
    {
        // Verificación de seguridad para currentEnemyAttack
        if (currentEnemyAttack == null || currentEnemyAttack.Count == 0)
        {
            Debug.LogWarning("No hay ataque enemigo para aplicar daño");
            isPlayerTurn = true;
            return;
        }

        int damage = 0;

        // Calcular daño total del ataque enemigo
        foreach (Card c in currentEnemyAttack)
        {
            if (c != null) // Verificación adicional para cartas nulas
            {
                damage += c.number;
            }
            else
            {
                Debug.LogWarning("Se encontró una carta nula en el ataque enemigo");
            }
        }

        // Verificar si el escudo está activo
        if (shieldActive)
        {
            damage = 0;
            Debug.Log("¡Escudo activado! Daño negado.");

            if (enemyActionInTurnText != null)
            {
                enemyActionInTurnText.text = "Bloqueaste todo el daño!";
            }

            shieldActive = false;

            if (shieldImage != null)
            {
                shieldImage.SetActive(false);
            }
        }

        // Aplicar daño al jugador (con verificación de GameManager)
        if (GameManager.Instance != null)
        {
            GameManager.Instance.playerHealth -= damage;
            ShowTemporaryEnemyMessage($"El enemigo te hizo {damage} de daño");
            Debug.Log($"¡El enemigo te ha hecho {damage} de daño!");
        }
        else
        {
            Debug.LogError("GameManager.Instance es nulo! No se puede aplicar daño");
        }

        // Limpiar el ataque enemigo después de aplicarlo
        currentEnemyAttack = null;
    }

    private bool PlayerCanReflect()
    {
        if (currentEnemyAttack == null || currentEnemyAttack.Count == 0)
            return false;

        List<Card> playerCards = CardInventory.Instance.cards;
        List<Card> enemyAttack = currentEnemyAttack;

        if (enemyAttack.Count > 0)
        {
            Card lastEnemyCard = enemyAttack[enemyAttack.Count - 1];

            foreach (Card playerCard in playerCards)
            {
                bool canReflectBySuit = playerCard.suit == lastEnemyCard.suit && playerCard.number == lastEnemyCard.number + 1;

                if (enemyAttack.Count == 1)
                {
                    if (playerCard.number == lastEnemyCard.number && playerCard.suit != lastEnemyCard.suit)
                    {
                        return true;
                    }
                }
                else if (canReflectBySuit)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool EnemyCanReflectAttack()
    {
        if (playerAttackHistory == null || playerAttackHistory.Count == 0 || currentEnemy.enemyInventory.Count == 0)
            return false;

        List<Card> enemyCards = currentEnemy.enemyInventory;
        Card lastPlayerCard = playerAttackHistory[playerAttackHistory.Count - 1];
        Card reflectingCard = null;

        foreach (Card enemyCard in enemyCards)
        {
            bool canReflectBySuit = enemyCard.suit == lastPlayerCard.suit && enemyCard.number == lastPlayerCard.number + 1;
            bool canReflectByNumber = enemyCard.number == lastPlayerCard.number && enemyCard.suit != lastPlayerCard.suit;

            if (playerAttackHistory.Count == 1)
            {
                if (canReflectByNumber)
                {
                    reflectingCard = enemyCard;
                    break;
                }
            }
            else if (canReflectBySuit)
            {
                reflectingCard = enemyCard;
                break;
            }
        }

        if (reflectingCard != null)
        {
            ReflectDamageToPlayer(reflectingCard);
            DisplayEnemyReflectCard(reflectingCard);
            return true;
        }

        return false;
    }

    private void ReflectDamageToPlayer(Card enemyCard)
    {
        int playerAttackSum = 0;
        foreach (Card card in playerAttackHistory)
        {
            playerAttackSum += card.number;
        }

        int reflectedDamage = Mathf.CeilToInt((playerAttackSum + enemyCard.number) / 2f);
        GameManager.Instance.playerHealth -= reflectedDamage;
        ShowTemporaryEnemyMessage($"El enemigo te reflejó {reflectedDamage} de daño");
        Debug.Log($"¡El enemigo te ha reflejado {reflectedDamage} de daño!");
    }

    private void DisplayEnemyReflectCard(Card card)
    {
        if (enemyCardReflectZone != null && enemyCardPrefab != null)
        {
            GameObject cardGO = Instantiate(enemyCardPrefab, enemyCardReflectZone.transform);
            cardGO.transform.localPosition = Vector3.zero;
            cardGO.transform.localScale = Vector3.one;

            EnemyCardUI enemyCardUI = cardGO.GetComponent<EnemyCardUI>();
            if (enemyCardUI != null)
            {
                enemyCardUI.Setup(card);
            }
            else
            {
                Debug.LogError("No se encontró el componente EnemyCardUI en el prefab de la carta.");
            }

            if (clearReflectCardCoroutine != null)
                StopCoroutine(clearReflectCardCoroutine);
            clearReflectCardCoroutine = StartCoroutine(ClearReflectCardAfterDelay(3f, cardGO));
        }
        else
        {
            Debug.LogError("enemyCardReflectZone o enemyCardPrefab no están asignados.");
        }
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

    public void ClearPlayerAttackHistory()
    {
        playerAttackHistory = new List<Card>();
        Debug.Log("Historial de ataque del jugador borrado");
    }


    public void EndEnemyTurnAfterReflect(int reflectedDamage)
    {
        currentEnemy.health -= reflectedDamage;
        canReflect = false;
        isPlayerTurn = true;
        isEnemyTurn = false;
        ClearEnemyAttackImmediately();
    }

    private IEnumerator HideEnemyCardsAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (enemyUsedText != null)
        {
            enemyUsedText.SetActive(false);
        }
        ClearEnemyAttackImmediately();
    }

    private IEnumerator ShowPlayerActionText(string message, float duration)
    {
        if (playerActionInTurnText != null)
        {
            if (hidePlayerTextCoroutine != null)
                StopCoroutine(hidePlayerTextCoroutine);
            playerActionInTurnText.text = message;
            yield return new WaitForSeconds(duration);
            playerActionInTurnText.text = "";
        }
    }

    private IEnumerator ShowEnemyActionText(string message, float duration)
    {
        if (enemyActionInTurnText != null)
        {
            if (hideEnemyTextCoroutine != null)
                StopCoroutine(hideEnemyTextCoroutine);
            enemyActionInTurnText.text = message;
            yield return new WaitForSeconds(duration);
            enemyActionInTurnText.text = "";
        }
    }

    public void ShowTemporaryPlayerMessage(string message)
    {
        hidePlayerTextCoroutine = StartCoroutine(ShowPlayerActionText(message, 3f));
    }

    private void ShowTemporaryEnemyMessage(string message)
    {
        hideEnemyTextCoroutine = StartCoroutine(ShowEnemyActionText(message, 3f));
    }

    public void ActivateShield()
    {
        if (!shieldActive)
        {
            shieldActive = true;
            shieldImage.SetActive(true);
        }
        else
        {
            CardInventoryUI.Instance.ShowSpecialCardMesage("Ya usaste el escudo en este turno");
        }
    }

    public void ActivateSword()
    {
        if (!swordActive)
        {
            swordActive = true;

            swordImage.SetActive(true);
        }
        else
        {
            CardInventoryUI.Instance.ShowSpecialCardMesage("Ya usaste la Espada en este turno");
        }
    }

    private IEnumerator HideShieldEffect()
    {
        yield return new WaitForSeconds(2f);
        enemyActionInTurnText.text = "";
        shieldImage.SetActive(false);
    }
    private void DisplayEnemyInventory()
    {
        if (enemyFullInventoryZone == null || enemyCardPrefab == null)
        {
            Debug.LogError("enemyFullInventoryZone o enemyCardPrefab no están asignados en BattleManager.");
            return;
        }

        // Limpiar el inventario mostrado anteriormente
        foreach (Transform child in enemyFullInventoryZone.transform)
        {
            Destroy(child.gameObject);
        }
        displayedInventoryCards.Clear();

        // Mostrar las cartas del inventario del enemigo
        foreach (Card card in currentEnemy.enemyInventory)
        {
            GameObject cardGO = Instantiate(enemyCardPrefab, enemyFullInventoryZone.transform);
            cardGO.transform.localPosition = Vector3.zero;
            cardGO.transform.localScale = Vector3.one;

            EnemyCardUI enemyCardUI = cardGO.GetComponent<EnemyCardUI>();
            if (enemyCardUI != null)
            {
                enemyCardUI.Setup(card);
                displayedInventoryCards.Add(cardGO); // Mantener referencia para destruir luego si es necesario
            }
            else
            {
                Debug.LogError("No se encontró el componente EnemyCardUI en el prefab de la carta.");
                Destroy(cardGO); // Limpiar la instancia si no se puede configurar
            }
        }
    }

    public void ClearEnemyInventoryDisplay()
    {
        foreach (GameObject cardGO in displayedInventoryCards)
        {
            Destroy(cardGO);
        }
        displayedInventoryCards.Clear();
    }


    private IEnumerator ClearReflectCardAfterDelay(float delay, GameObject cardGO)
    {
        yield return new WaitForSeconds(delay);
        Destroy(cardGO);
    }

    public void EndRun()
    {
        currentEnemy.health = currentEnemy.maxHealth;

        SceneManager.LoadScene("MainMenu");


    }
}