using System.Collections.Generic;
using UnityEngine;

public class CardInventory : MonoBehaviour
{
    public static CardInventory Instance { get; private set; }

    private const int MaxCards = 7;

    public List<Card> cards = new List<Card>();

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

    public void AddRandomCard()
    {
        if (cards.Count >= MaxCards)
        {
            Debug.Log("Inventory full.");
            return;
        }

        int number = Random.Range(1, 11); // 1 to 10 inclusive
        Suit suit = (Suit)Random.Range(0, System.Enum.GetValues(typeof(Suit)).Length);

        Card newCard = new Card(number, suit);
        cards.Add(newCard);

        Debug.Log($"Added card: {number} of {suit}");


        if(BattleManager.Instance.inventoryGenerated){

            BattleManager.Instance.isPlayerTurn = false;
        }
    }

public void RemoveCard(Card cardToRemove)
{
    for (int i = 0; i < cards.Count; i++)
    {
        if (cards[i].suit == cardToRemove.suit && cards[i].number == cardToRemove.number)
        {
            cards.RemoveAt(i);
            break;
        }
    }
}


    public List<Card> GetCards()
    {
        return new List<Card>(cards);
    }

    public void ClearInventory()
    {
        cards.Clear();
    }
}