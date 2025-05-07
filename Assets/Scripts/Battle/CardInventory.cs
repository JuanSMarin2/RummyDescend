using System.Collections.Generic;
using UnityEngine;

public class CardInventory : MonoBehaviour
{
    public static CardInventory Instance { get; private set; }

    private const int MaxCards = 7;
    public int maxCards { get { return MaxCards; } } // Hacer la capacidad máxima accesible

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

    public void AddCard(Card cardToAdd)
    {
        if (cards.Count < MaxCards)
        {
            cards.Add(cardToAdd);
            Debug.Log($"Added card: {cardToAdd}");
        }
        else
        {
            Debug.Log("Card inventory is full.");
        }
    }

    public void AddRandomCard()
    {
        if (cards.Count < MaxCards)
        {
            int randomNum = Random.Range(1, 11);
            Suit randomSuit = (Suit)Random.Range(0, System.Enum.GetValues(typeof(Suit)).Length);
            cards.Add(new Card(randomNum, randomSuit));
        }
        else
        {
            Debug.Log("Card inventory is full.");
        }
    }

    public void RemoveCard(Card cardToRemove)
    {
        cards.Remove(cardToRemove);
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