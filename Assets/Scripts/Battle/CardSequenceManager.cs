using System.Collections.Generic;
using UnityEngine;

public class CardSequenceManager : MonoBehaviour
{

    [SerializeField] private GameObject dragButton; 
     [SerializeField] private GameObject revertButton; 

     [SerializeField] private GameObject attackButton; 



    [SerializeField] private Transform[] dropZones; // 0, 1, 2
    [SerializeField] private Transform originalParent; // Donde vuelven al hacer revertir

    private List<Card> currentSequence = new List<Card>(); // Lo que hay en cada slot

    public static CardSequenceManager Instance { get; private set; }

    void Start(){

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

    public bool TryPlaceCard(GameObject cardGO, Card cardData)
    {
            int currentIndex = currentSequence.Count;

    if (currentIndex == 0)
    {
        PlaceCard(cardGO, cardData, 0);
        return true;
    }

    Card lastCard = currentSequence[currentIndex - 1];

    bool validBySuit = lastCard.suit == cardData.suit && cardData.number == lastCard.number + 1;
    bool validByNumber = lastCard.number == cardData.number && lastCard.suit != cardData.suit;

    // Validación adicional si estamos colocando en la tercera posición
    if (currentIndex == 2)
    {
        Card firstCard = currentSequence[0];

        bool validWithFirstBySuit = firstCard.suit == cardData.suit && cardData.number == currentSequence[1].number + 1;
        bool validWithFirstByNumber = firstCard.number == cardData.number && firstCard.suit != cardData.suit;

        // Ambas condiciones (con segunda y con primera) deben cumplirse
        if ((validBySuit || validByNumber) && (validWithFirstBySuit || validWithFirstByNumber))
        {
            PlaceCard(cardGO, cardData, currentIndex);
            return true;
        }

        return false;
    }

    if (validBySuit || validByNumber)
    {
        PlaceCard(cardGO, cardData, currentIndex);
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

    if (dragButton != null)
        dragButton.SetActive(true);

    if (revertButton != null)
        revertButton.SetActive(false);

    if (attackButton != null)
        attackButton.SetActive(false);
}


    private void PlaceCard(GameObject cardGO, Card cardData, int index)
    {
        cardGO.transform.SetParent(dropZones[index], false);
        currentSequence.Add(cardData);

        if (dragButton != null)
    {
        dragButton.SetActive(false);
    }

          if (revertButton != null)
    {
        revertButton.SetActive(true);
    }

              if (attackButton != null)
    {
        attackButton.SetActive(true);
    }

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

    if (dragButton != null)
    {
        dragButton.SetActive(true); 
    }

        if (revertButton != null)
    {
        revertButton.SetActive(false); 
    }

          if (attackButton != null)
    {
        attackButton.SetActive(false); 
    }


        currentSequence.Clear();
    }
}
