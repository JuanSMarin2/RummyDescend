using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CardInventoryUI : MonoBehaviour
{
    public static CardInventoryUI Instance { get; private set; }

    [SerializeField] public GameObject cardPrefab;
    [SerializeField] private Transform cardContainer;
    [SerializeField] private TextMeshProUGUI isInventoryFullToWildCardText; // Referencia al texto

    [SerializeField] public GameObject wildCardToNumberPanel;
    [SerializeField] public GameObject wildCardToSuitPanel;

    [SerializeField] public GameObject enemyInventoryPanel;

    [SerializeField] public GameObject seeEnemyCardsButton;

    private float targetAlpha = 220f / 255f; // Normalizamos el valor alfa al rango 0-1

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


    public void SeeEnemyInventoryPanel()
    {
        enemyInventoryPanel.SetActive(true);
    }

    public void CloseEnemyInventoryPanel()
    {
        enemyInventoryPanel.SetActive(false);
    }

    public void RefreshUI()
    {
        ClearUI();

        foreach (Card card in CardInventory.Instance.GetCards())
        {
            CreateCardVisual(card);
        }
    }

    public void SetSpecialCardsPanelColor(Color color)
    {
        if (wildCardToNumberPanel != null)
        {
            Image panelImage = wildCardToNumberPanel.GetComponent<Image>();
            if (panelImage != null)
            {
                // Creamos un nuevo color basado en el color proporcionado, pero con el alfa deseado
                Color newColor = new Color(color.r, color.g, color.b, targetAlpha);
                panelImage.color = newColor;
            }
            else
            {
                Debug.LogError("El GameObject specialCardsPanel no tiene un componente Image.");
            }
        }
        else
        {
            Debug.LogError("La referencia a specialCardsPanel no está asignada.");
        }
    }

    public void DragCrad()
    {
        CardInventory.Instance.AddRandomCard();
        Debug.Log("Arrastro");
        BattleManager.Instance.isPlayerTurn = false;
        BattleManager.Instance.isEnemyTurn = true;
        Debug.Log("Turnos cambiados");
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

        // 👉 Aquí llamas a Setup con los datos correctos
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


    public void ShowSpecialCardMesage(string mesage)
    {
        if (isInventoryFullToWildCardText != null)
        {
            isInventoryFullToWildCardText.text = mesage;
            StartCoroutine(ClearInventoryFullWildCardMessageAfterDelay(1f));
        }
    }

 

    private IEnumerator ClearInventoryFullWildCardMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (isInventoryFullToWildCardText != null)
        {
            isInventoryFullToWildCardText.text = "";
        }
    }
}