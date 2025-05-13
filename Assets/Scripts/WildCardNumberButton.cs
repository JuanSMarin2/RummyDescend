using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WildCardNumberButton : MonoBehaviour
{
    private int cardNumber;
    private Suit wildCardSuit;
    [SerializeField] private GameObject wildCardToNumberPanel;
    private bool firstClick = true;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnNumberButtonClick);

        // Obtener el número del botón del texto
        TextMeshProUGUI buttonText = GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null && int.TryParse(buttonText.text, out cardNumber))
        {
            // El número se obtiene del texto del botón
        }
        else
        {
            Debug.LogError("No se pudo obtener el número del botón: " + gameObject.name);
        }
    }

    public void SetWildCardSuit(Suit suit)
    {
        wildCardSuit = suit;
    }

    public void OnNumberButtonClick()
    {
        if (firstClick)
        {
            if (CardInventory.Instance.cards.Count < CardInventory.Instance.maxCards)
            {
                CardInventory.Instance.AddCard(new Card(cardNumber, wildCardSuit));
                CardInventoryUI.Instance.RefreshUI();

                if (wildCardToNumberPanel != null)
                {
                    wildCardToNumberPanel.SetActive(false);
                }
            }
            else
            {
                Debug.Log("Inventario de cartas lleno (desde botón de número).");
                // El mensaje de inventario lleno ya se muestra al hacer clic en la WildCard
            }
            
            firstClick = false;
        }
        else
        {
            firstClick = true;
        }
    }
}