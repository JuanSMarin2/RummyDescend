using System.Collections.Generic;
using UnityEngine;

public class SpecialCardInventory : MonoBehaviour
{
    public static SpecialCardInventory Instance { get; private set; }

    public int MaxCards = 5; // Smaller capacity than regular cards
    public List<SpecialCard> cards = new List<SpecialCard>();




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

    public void AddCard(SpecialCard cardToAdd)
    {
        if (cards.Count >= MaxCards)
        {
            Debug.Log("Special card inventory full.");
            return;
        }

        cards.Add(cardToAdd);
        Debug.Log($"Added special card: {cardToAdd.type}");
    }


    public void AddRandomSpecialCard()
    {
        if (cards.Count >= MaxCards)
        {
            Debug.Log("Special card inventory full.");
            return;
        }

        // Weighted probabilities for different card types
        SpecialCardType type = GetRandomCardType();
        SpecialCard newCard = new SpecialCard(type);
        cards.Add(newCard);

        Debug.Log($"Added special card: {type}");
    }

    private SpecialCardType GetRandomCardType()
    {
        float rand = Random.value;

        if (rand < 0.11f) return SpecialCardType.RedWildCard;
        if (rand < 0.22f) return SpecialCardType.GreenWildCard;
        if (rand < 0.33f) return SpecialCardType.PinkWildCard;
        if (rand < 0.44f) return SpecialCardType.WhiteWildCard;
        if (rand < 0.55f) return SpecialCardType.Protector;
        if (rand < 0.66f) return SpecialCardType.DoubleAttack;
        if (rand < 0.79f) return SpecialCardType.Healer;
        if (rand < 0.92f) return SpecialCardType.Lens;
        return SpecialCardType.WildCard;
    }

    public void RemoveCard(SpecialCard cardToRemove)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].type == cardToRemove.type)
            {
                cards.RemoveAt(i);
                break;
            }
        }
    }

    public List<SpecialCard> GetCards()
    {
        return new List<SpecialCard>(cards);
    }

    public void ClearInventory()
    {
        cards.Clear();
    }

    public SpecialCardType GetLatestCardType()
    {
        if (cards.Count > 0)
        {
            return cards[cards.Count - 1].type;
        }
        return SpecialCardType.WildCard; // Valor por defecto
    }

}