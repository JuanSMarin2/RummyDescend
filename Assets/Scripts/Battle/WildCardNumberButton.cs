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

        // Obtener el n�mero del bot�n del texto
        TextMeshProUGUI buttonText = GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null && int.TryParse(buttonText.text, out cardNumber))
        {
            // El n�mero se obtiene del texto del bot�n
        }
        else
        {
            Debug.LogError("No se pudo obtener el n�mero del bot�n: " + gameObject.name);
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
                Debug.Log("Inventario de cartas lleno (desde bot�n de n�mero).");
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