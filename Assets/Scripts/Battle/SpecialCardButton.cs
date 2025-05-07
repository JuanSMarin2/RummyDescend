using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class SpecialCardButton : MonoBehaviour
{
    private SpecialCard cardData;
    private Transform originalParent;
    private Button button;
    private bool firstPress = true;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    private void Start()
    {

    }

    public void Setup(SpecialCard card, Transform originalLayout)
    {
        cardData = card;
        originalParent = originalLayout;
    }

    public void OnClick()
    {
        if (firstPress)
        {
            Debug.Log("Clicked special card: " + cardData.type);

            // Verificar si es un comodín antes de comprobar el inventario
            if (IsWildCard(cardData.type) && CardInventory.Instance.cards.Count >= CardInventory.Instance.maxCards)
            {
                // Delegamos la visualización del mensaje a CardInventoryUI
                if (CardInventoryUI.Instance != null)
                {
                    CardInventoryUI.Instance.ShowSpecialCardMesage("Inventario de cartas lleno");
                }
                return;
            }

            // Implement special card effects here
            Debug.Log(cardData.type);
            switch (cardData.type)
            {
                case SpecialCardType.RedWildCard:
                    CardInventoryUI.Instance.SetSpecialCardsPanelColor(Color.red);
                    if (CardInventoryUI.Instance.wildCardToNumberPanel != null)
                    {
                        Debug.Log("in case RedWildCard");
                        CardInventoryUI.Instance.wildCardToNumberPanel.SetActive(true);
                        SpecialCardInventoryUI.Instance.ToggleSpecialCardsPanel();
                        AssignWildCardSuitToNumberPanel(cardData.type);
                    }
                    break;

                case SpecialCardType.GreenWildCard:
                    CardInventoryUI.Instance.SetSpecialCardsPanelColor(Color.green);
                    if (CardInventoryUI.Instance.wildCardToNumberPanel != null)
                    {
                        Debug.Log("in case GreenWildCard");
                        CardInventoryUI.Instance.wildCardToNumberPanel.SetActive(true);
                        SpecialCardInventoryUI.Instance.ToggleSpecialCardsPanel();
                        AssignWildCardSuitToNumberPanel(cardData.type);
                    }
                    break;

                case SpecialCardType.PinkWildCard:
                    CardInventoryUI.Instance.SetSpecialCardsPanelColor(new Color(1f, 0.5f, 0.7f)); // Color rosa
                    if (CardInventoryUI.Instance.wildCardToNumberPanel != null)
                    {
                        Debug.Log("in case PinkWildCard");
                        CardInventoryUI.Instance.wildCardToNumberPanel.SetActive(true);
                        SpecialCardInventoryUI.Instance.ToggleSpecialCardsPanel();
                        AssignWildCardSuitToNumberPanel(cardData.type);
                    }
                    break;

                case SpecialCardType.WhiteWildCard:
                    CardInventoryUI.Instance.SetSpecialCardsPanelColor(Color.gray);
                    if (CardInventoryUI.Instance.wildCardToNumberPanel != null)
                    {
                        Debug.Log("in case WhiteWildCard");
                        CardInventoryUI.Instance.wildCardToNumberPanel.SetActive(true);
                        SpecialCardInventoryUI.Instance.ToggleSpecialCardsPanel();
                        AssignWildCardSuitToNumberPanel(cardData.type);
                    }
                    break;

                case SpecialCardType.WildCard:
                    if (CardInventoryUI.Instance.wildCardToSuitPanel != null)
                    {
                        Debug.Log("in case WildCard (pura)");
                        CardInventoryUI.Instance.wildCardToSuitPanel.SetActive(true);
                        SpecialCardInventoryUI.Instance.ToggleSpecialCardsPanel();
                    }
                    break;

                case SpecialCardType.Lens:
                    if (!BattleManager.Instance.hasLens)
                    {
                        CardInventoryUI.Instance.seeEnemyCardsButton.SetActive(true);
                        SpecialCardInventory.Instance.RemoveCard(cardData);
                        SpecialCardInventoryUI.Instance.ToggleSpecialCardsPanel();
                        BattleManager.Instance.hasLens = true;
                        BattleManager.Instance.isPlayerTurn = false;
                        BattleManager.Instance.isEnemyTurn = true;
                    }
                    else
                    {
                        CardInventoryUI.Instance.ShowSpecialCardMesage("Ya usaste la lupa en este combate");
                    }
                    break;

                case SpecialCardType.Protector:
                    BattleManager.Instance.ActivateShield(); // Activar el escudo en BattleManager
                    SpecialCardInventory.Instance.RemoveCard(cardData);
                    SpecialCardInventoryUI.Instance.RefreshUI();
                    CardInventoryUI.Instance.ShowSpecialCardMesage("Escudo Activado");
                    break;

                case SpecialCardType.DoubleAttack:
                    BattleManager.Instance.ActivateSword();
                    SpecialCardInventory.Instance.RemoveCard(cardData);
                    SpecialCardInventoryUI.Instance.RefreshUI();
                    CardInventoryUI.Instance.ShowSpecialCardMesage("Espada Activada");
                    break;

                case SpecialCardType.Healer:

                    if (GameManager.Instance.playerHealth < UIHealthManager.Instance.maxPlayerHealth)
                    {

                         int oldHealth = GameManager.Instance.playerHealth;

                         GameManager.Instance.playerHealth += 20;

                         int healedHealth = GameManager.Instance.playerHealth - oldHealth;




                        SpecialCardInventoryUI.Instance.ToggleSpecialCardsPanel();
                        SpecialCardInventory.Instance.RemoveCard(cardData);

                        BattleManager.Instance.ShowTemporaryPlayerMessage($":Curaste {healedHealth} de vida");

                        BattleManager.Instance.isPlayerTurn = false;
                        BattleManager.Instance.isEnemyTurn = true;
                    }
                    else
                    {
                        CardInventoryUI.Instance.ShowSpecialCardMesage("Tienes toda la vida");
                    }

                    break;

                default:
                    Debug.LogWarning("Tipo de carta especial no manejado en este switch: " + cardData.type);
                    break;
            }

            // Las WildCards se remueven después de activar el panel
            if (IsWildCard(cardData.type))
            {
                SpecialCardInventory.Instance.RemoveCard(cardData);
                SpecialCardInventoryUI.Instance.RefreshUI();
            }
            firstPress = false;
        }
        else
        {
            firstPress = true;
        }
    }

    private bool IsWildCard(SpecialCardType type)
    {
        return type == SpecialCardType.RedWildCard ||
               type == SpecialCardType.GreenWildCard ||
               type == SpecialCardType.PinkWildCard ||
               type == SpecialCardType.WhiteWildCard ||
               type == SpecialCardType.WildCard;
    }

    private void AssignWildCardSuitToNumberPanel(SpecialCardType wildCardType)
    {
        if (CardInventoryUI.Instance.wildCardToNumberPanel != null)
        {
            WildCardNumberButton[] numberButtons = CardInventoryUI.Instance.wildCardToNumberPanel.GetComponentsInChildren<WildCardNumberButton>();
            Suit assignedSuit = Suit.White; // Valor por defecto

            switch (wildCardType)
            {
                case SpecialCardType.RedWildCard:
                    assignedSuit = Suit.Red;
                    break;
                case SpecialCardType.GreenWildCard:
                    assignedSuit = Suit.Green;
                    break;
                case SpecialCardType.PinkWildCard:
                    assignedSuit = Suit.Pink;
                    break;
                case SpecialCardType.WhiteWildCard:
                    assignedSuit = Suit.White;
                    break;
            }

            foreach (var button in numberButtons)
            {
                button.SetWildCardSuit(assignedSuit);
            }
        }
    }

    public void ReturnToOriginal()
    {
        transform.SetParent(originalParent, false);
    }

    public SpecialCard GetCardData()
    {
        return cardData;
    }
}