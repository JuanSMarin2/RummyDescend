using UnityEngine;
using TMPro;

public class BattleShop : MonoBehaviour
{
    [Header("Configuración UI")]
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private float feedbackDuration = 2f;
    [SerializeField] private GameObject infoPanel;

    [Header("Precios")]
    [SerializeField] private int specialCardPrice = 20;
    [SerializeField] private int healthExchangeRate = 5; // Vida por monedas

    private void Start()
    {
        infoPanel.SetActive(false);
        UpdateAllUI();
    }

    private void Update()
    {
        UpdateAllUI();
    }

    private void UpdateAllUI()
    {
        UpdateCoinsText();
        UpdateHealthText();
    }

    private void UpdateCoinsText()
    {
        if (coinsText != null)
        {
            coinsText.text = $"x {MoneyManager.Instance.coins}";
        }
    }

    private void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = $"HP: {GameManager.Instance.playerHealth}";
        }
    }

    public void PurchaseSpecialCard()
    {
        if (MoneyManager.Instance.coins >= specialCardPrice)
        {
            MoneyManager.Instance.RemoveCoins(specialCardPrice);
            SpecialCardInventory.Instance.AddRandomSpecialCard();
            SpecialCardInventoryUI.Instance.RefreshUI();
            ShowFeedback("¡Carta especial obtenida!", Color.green);
        }
        else
        {
            ShowFeedback("No tienes suficientes monedas", Color.red);
        }
        UpdateAllUI();
    }

    public void ExchangeHealthForCoins()
    {
        // Verificar si tiene suficiente salud
        if (GameManager.Instance.playerHealth > healthExchangeRate)
        {
            // Perder salud y ganar monedas
            GameManager.Instance.playerHealth -= healthExchangeRate;
            MoneyManager.Instance.IncreaseMoney(healthExchangeRate);

            ShowFeedback($"+{healthExchangeRate} monedas", Color.yellow);
        }
        else
        {
            ShowFeedback("No tienes suficiente salud", Color.red);
        }
        UpdateAllUI();
    }

    private void ShowFeedback(string message, Color color)
    {
        if (feedbackText != null)
        {
            feedbackText.color = color;
            feedbackText.text = message;
            CancelInvoke(nameof(ClearFeedback));
            Invoke(nameof(ClearFeedback), feedbackDuration);
        }
    }

    private void ClearFeedback()
    {
        if (feedbackText != null)
        {
            feedbackText.text = "";
        }
    }

    public void TogglePanel()
    {
        infoPanel.SetActive(!infoPanel.activeSelf);
        if (infoPanel.activeSelf)
        {
            UpdateAllUI();
        }
    }
}