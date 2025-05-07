using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SeeInventoryInLevel : MonoBehaviour
{
    public static SeeInventoryInLevel Instance { get; private set; }

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardContainer;

    [SerializeField] private GameObject CardInventoryPanel;

    private List<GameObject> cardVisuals = new List<GameObject>();

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
        CardInventoryPanel.SetActive(false);
    }

    public void RefreshUI()
    {
        ClearUI();

        foreach (Card card in CardInventory.Instance.GetCards())
        {
            CreateCardVisual(card);
        }
    }

    private void ClearUI()
    {
        foreach (GameObject visual in cardVisuals)
        {
            Destroy(visual);
        }
        cardVisuals.Clear();
    }

    private void CreateCardVisual(Card card)
    {
        GameObject newCardGO = Instantiate(cardPrefab, cardContainer);

        // Asignar número visual
        TextMeshProUGUI numberText = newCardGO.transform.Find("NumberText").GetComponent<TextMeshProUGUI>();
        numberText.text = card.number.ToString();

        // Asignar color
        Image cardImage = newCardGO.GetComponent<Image>();
        cardImage.color = GetColorBySuit(card.suit);

        //  Aquí llamas a Setup con los datos correctos
        CardButton cardButton = newCardGO.GetComponent<CardButton>();
        cardButton.Setup(card, cardContainer); // Usamos el container como parent original

        cardVisuals.Add(newCardGO);
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

    public void OpenCardInventoryPanel()
    {
        RefreshUI();
        CardInventoryPanel.SetActive(true);
    }


    public void Back()
    {
        CardInventoryPanel.SetActive(false);

    }
}
