using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardButton : MonoBehaviour
{
    private Card cardData;
    private Transform originalParent;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

     
    }

    public void Setup(Card card, Transform originalLayout)
    {
        cardData = card;
        originalParent = originalLayout;
    }

public void OnClick()
{
    if (cardData == null)
    {
        Debug.LogError("cardData es null. ¿Olvidaste llamar a Setup()?");
        return;
    }

    Debug.Log("Clic en carta: " + cardData.number + " " + cardData.suit);

    if (CardSequenceManager.Instance == null)
    {
        Debug.LogError("CardSequenceManager.Instance es null");
        return;
    }

    // --- REFLECTAJE ---
    if (BattleManager.Instance != null && BattleManager.Instance.IsReflectAvailable())
    {
        Card lastEnemyCard = BattleManager.Instance.GetLastEnemyCard();

        bool validBySuit = cardData.suit == lastEnemyCard.suit && cardData.number == lastEnemyCard.number + 1;
        bool validByNumber = cardData.number == lastEnemyCard.number && cardData.suit != lastEnemyCard.suit;

        if (validBySuit || validByNumber)
        {
            ReflectWithCard(cardData);
            return;
        }
    }

    // --- Modo normal (ataque del jugador) ---
    bool placed = CardSequenceManager.Instance.TryPlaceCard(gameObject, cardData);
}


private void ReflectWithCard(Card reflectCard)
{
    Debug.Log("¡Ataque reflejado con: " + reflectCard.number + " " + reflectCard.suit + "!");

    // Calcular daño del enemigo
    var enemyAttack = BattleManager.Instance.GetCurrentEnemyAttack();
    int enemyAttackSum = 0;
    foreach (Card card in enemyAttack)
        enemyAttackSum += card.number;

    int totalReflectDamage = Mathf.CeilToInt((enemyAttackSum + reflectCard.number) / 2f);

    Debug.Log($"Daño reflejado al enemigo: {totalReflectDamage}");

    BattleManager.Instance.DamageEnemy(totalReflectDamage);

    // Remueve la carta usada para reflejar
    CardInventory.Instance.RemoveCard(reflectCard);

    // Fin del turno enemigo, regresa al jugador
    BattleManager.Instance.EndEnemyTurnAfterReflect();
}



    public void ReturnToOriginal()
    {
        transform.SetParent(originalParent, false);
    }

    public Card GetCardData()
    {
        return cardData;
    }
}
