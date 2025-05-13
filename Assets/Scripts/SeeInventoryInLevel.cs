using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeeInventoryInLevel : MonoBehaviour
{
    public static SeeInventoryInLevel Instance { get; private set; }

    [Header("Configuración General")]
    [SerializeField] private GameObject inventoryPanel;


    [Header("Cartas Normales")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform normalCardsContainer;

    [Header("Cartas Especiales")]
    [SerializeField] private GameObject specialCardPrefab;
    [SerializeField] private Transform specialCardsContainer;
    [SerializeField] private Sprite[] specialCardSprites;

    private List<GameObject> normalCardVisuals = new List<GameObject>();
    private List<GameObject> specialCardVisuals = new List<GameObject>();

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
        inventoryPanel.SetActive(false);
     
    }


    public void Activate()
    {
        RefreshAllCards();
        inventoryPanel.SetActive(true);
       
    }

    public void Deactivate()
    {
        inventoryPanel.SetActive(false);
    }


    public void ToggleInventory()
    {
  
        bool isActive = !inventoryPanel.activeSelf;
        inventoryPanel.SetActive(isActive);

        if (isActive)
        {
            RefreshAllCards();
        }
    }

    public void RefreshAllCards()
    {
        RefreshNormalCards();
        RefreshSpecialCards();
    }

    public void RefreshNormalCards()
    {
        ClearCardVisuals(normalCardVisuals);

        foreach (Card card in CardInventory.Instance.GetCards())
        {
            CreateNormalCardVisual(card);
        }
    }

    public void RefreshSpecialCards()
    {
        ClearCardVisuals(specialCardVisuals);

        foreach (SpecialCard card in SpecialCardInventory.Instance.GetCards())
        {
            CreateSpecialCardVisual(card);
        }
    }

    private void CreateNormalCardVisual(Card card)
    {
        GameObject cardObj = Instantiate(cardPrefab, normalCardsContainer);

        // Configurar visual
        TextMeshProUGUI numberText = cardObj.transform.Find("NumberText").GetComponent<TextMeshProUGUI>();
        numberText.text = card.number.ToString();

        Image cardImage = cardObj.GetComponent<Image>();
        cardImage.color = GetColorBySuit(card.suit);

        // Configurar interacción
        CardButton cardButton = cardObj.GetComponent<CardButton>();
        cardButton.Setup(card, normalCardsContainer);

        normalCardVisuals.Add(cardObj);
    }

    private void CreateSpecialCardVisual(SpecialCard card)
    {
        GameObject cardObj = Instantiate(specialCardPrefab, specialCardsContainer);

        // Configurar visual
        Image cardImage = cardObj.GetComponent<Image>();
        cardImage.sprite = GetSpecialCardSprite(card.type);

        TextMeshProUGUI nameText = cardObj.GetComponentInChildren<TextMeshProUGUI>();
        nameText.text = FormatSpecialCardName(card.type);

        // Configurar interacción
        SpecialCardButton specialButton = cardObj.GetComponent<SpecialCardButton>();
        specialButton.Setup(card, specialCardsContainer);

        specialCardVisuals.Add(cardObj);
    }

    private void ClearCardVisuals(List<GameObject> visuals)
    {
        foreach (GameObject visual in visuals)
        {
            if (visual != null) Destroy(visual);
        }
        visuals.Clear();
    }

    private Color GetColorBySuit(Suit suit)
    {
        switch (suit)
        {
            case Suit.Red: return Color.red;
            case Suit.Pink: return new Color(1f, 0.5f, 0.7f);
            case Suit.Green: return Color.green;
            case Suit.White: return Color.white;
            default: return Color.white;
        }
    }

    private Sprite GetSpecialCardSprite(SpecialCardType type)
    {
        int index = (int)type;
        if (index >= 0 && index < specialCardSprites.Length)
        {
            return specialCardSprites[index];
        }
        return null;
    }

    private string FormatSpecialCardName(SpecialCardType type)
    {
        return type.ToString()
            .Replace("Card", "")
            .Replace("Wild", "W.")
            .Replace("Protector", "Escudo")
            .Replace("DoubleAttack", "Doble Ataque")
            .Replace("Healer", "Cura");
    }
}