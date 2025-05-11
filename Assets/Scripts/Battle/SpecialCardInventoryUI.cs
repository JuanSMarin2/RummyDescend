using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpecialCardInventoryUI : MonoBehaviour
{
    public static SpecialCardInventoryUI Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private GameObject specialCardPrefab;
    [SerializeField] private Transform cardContainer;
    [SerializeField] private Sprite[] cardSprites; // Asignar en el inspector en orden de enum

    [Header("Panel Management")]
    [SerializeField] public GameObject specialCardsPanel;
    [SerializeField] private GameObject specialCardsButton;

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
        RefreshUI();
    }

    public void ToggleSpecialCardsPanel()
    {
        Debug.Log("Toggle Special Cards Panel");
        bool isActive = !specialCardsPanel.activeSelf;
        specialCardsPanel.SetActive(isActive);
        specialCardsButton.SetActive(!isActive);

        if (isActive) RefreshUI(); // Actualizar UI al abrir
    }

  


    public void RefreshUI()
    {
        ClearUI();

        if (SpecialCardInventory.Instance == null)
        {
            Debug.LogError("SpecialCardInventory instance is missing!");
            return;
        }

        foreach (SpecialCard card in SpecialCardInventory.Instance.GetCards())
        {
            CreateCardVisual(card);
        }
    }

    private void ClearUI()
    {
        foreach (GameObject visual in cardVisuals)
        {
            if (visual != null) Destroy(visual);
        }
        cardVisuals.Clear();
    }

    private void CreateCardVisual(SpecialCard card)
    {
        if (specialCardPrefab == null || cardContainer == null)
        {
            Debug.LogError("Prefab or container not assigned!");
            return;
        }

        GameObject newCardGO = Instantiate(specialCardPrefab, cardContainer);

        // Configurar imagen de fondo
        Image cardImage = newCardGO.GetComponent<Image>();
        if (cardImage != null)
        {
            cardImage.sprite = GetSpriteForType(card.type);
        }

        // Buscar el TextMeshPro en los hijos
        TextMeshProUGUI nameText = newCardGO.GetComponentInChildren<TextMeshProUGUI>(true);
        if (nameText != null)
        {
            nameText.text = FormatCardName(card.type);
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI component not found in children!");
        }

        // Configurar botón para enviar a la secuencia (usando el nuevo script)
        SpecialCardButton sequenceButton = newCardGO.GetComponent<SpecialCardButton>();
        if (sequenceButton != null)
        {
            sequenceButton.Setup(card, cardContainer);
        }
        else
        {
            Debug.LogError("SpecialCardToSequenceButton component not found on special card prefab!");
        }

        cardVisuals.Add(newCardGO);
    }

    private string FormatCardName(SpecialCardType type)
    {
        // Formatear el nombre para mejor visualización
        return type.ToString().Replace("Card", "").Replace("Wild", "W.");
    }

    private Sprite GetSpriteForType(SpecialCardType type)
    {
        int index = (int)type;
        if (index >= 0 && index < cardSprites.Length)
        {
            return cardSprites[index];
        }
        Debug.LogWarning($"Sprite not found for type {type}");
        return null;
    }
}