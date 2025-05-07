using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSequenceManager : MonoBehaviour
{
    [SerializeField] public GameObject dragButton;
    [SerializeField] private GameObject revertButton;
    [SerializeField] private GameObject attackButton;
    [SerializeField] private Transform[] dropZones; // 0, 1, 2
    [SerializeField] private Transform originalParent; // Donde vuelven al hacer revertir

    private List<Card> currentSequence = new List<Card>(); // Lo que hay en cada slot

    public static CardSequenceManager Instance { get; private set; }

    void Start()
    {
        revertButton.SetActive(false);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public bool TryPlaceCard(GameObject cardGO, object cardData)
    {
        if (cardData is Card normalCard)
        {
            return TryPlaceNormalCard(cardGO, normalCard);
        }
        return false;
    }

    private bool TryPlaceNormalCard(GameObject cardGO, Card card)
    {
        int currentIndex = currentSequence.Count;

        if (currentIndex == 0)
        {
            PlaceCard(cardGO, card, 0);
            return true;
        }

        Card lastCard = currentSequence[currentIndex - 1];
        bool validBySuit = lastCard.suit == card.suit && card.number == lastCard.number + 1;
        bool validByNumber = lastCard.number == card.number && lastCard.suit != card.suit;

        if (currentIndex == 2)
        {
            Card firstCard = currentSequence[0];
            bool validWithFirstBySuit = firstCard.suit == card.suit && card.number == currentSequence[1].number + 1;
            bool validWithFirstByNumber = firstCard.number == card.number && firstCard.suit != card.suit;

            if ((validBySuit || validByNumber) && (validWithFirstBySuit || validWithFirstByNumber))
            {
                PlaceCard(cardGO, card, currentIndex);
                return true;
            }
            return false;
        }

        if (validBySuit || validByNumber)
        {
            PlaceCard(cardGO, card, currentIndex);
            return true;
        }
        return false;
    }

    public List<Card> GetCurrentSequence()
    {
        return currentSequence;
    }

    public void ClearPlayedCards()
    {
        for (int i = 0; i < dropZones.Length; i++)
        {
            foreach (Transform card in dropZones[i])
            {
                Destroy(card.gameObject); // Eliminar visualmente
            }
        }
        currentSequence.Clear();
        if (dragButton != null) dragButton.SetActive(true);
        if (revertButton != null) revertButton.SetActive(false);
        if (attackButton != null) attackButton.SetActive(false);
    }

    private void PlaceCard(GameObject cardGO, Card cardData, int index)
    {
        cardGO.transform.SetParent(dropZones[index], false);
        currentSequence.Add(cardData);
        if (dragButton != null) dragButton.SetActive(false);
        if (revertButton != null) revertButton.SetActive(true);
        if (attackButton != null) attackButton.SetActive(true);
    }

    public void RevertSequence()
    {
        for (int i = 0; i < dropZones.Length; i++)
        {
            foreach (Transform card in dropZones[i])
            {
                card.SetParent(originalParent, false);
            }
        }
        RevertSequenceUI();
        currentSequence.Clear();
    }

    public void RevertSequenceUI()
    {
        Debug.Log("Revert UI");
        if (dragButton != null) dragButton.SetActive(true);
        if (revertButton != null) revertButton.SetActive(false);
        if (attackButton != null) attackButton.SetActive(false);
    }

    // Nueva función para verificar la combinación de 3 cartas iguales
    public bool IsThreeOfAKind(List<Card> cards)
    {
        if (cards.Count != 3) return false;
        return (cards[0].number == cards[1].number && cards[1].number == cards[2].number);
    }

    // Nueva función para verificar la escalera de 3 cartas del mismo palo
    public bool IsStraightFlush(List<Card> cards)
    {
        if (cards.Count != 3) return false;
        // Primero ordenamos las cartas por número
        cards.Sort((a, b) => a.number.CompareTo(b.number));
        return (cards[0].suit == cards[1].suit && cards[1].suit == cards[2].suit &&
                cards[1].number == cards[0].number + 1 && cards[2].number == cards[1].number + 1);
    }
}