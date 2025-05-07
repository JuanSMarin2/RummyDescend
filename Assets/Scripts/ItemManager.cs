using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEngine.Rendering.DebugUI;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private int value = 1;
    [SerializeField] private TextMeshProUGUI pickupFeedbackText; // Asigna esto en el Inspector

    [SerializeField] GameObject shopPanel;

    private Coroutine feedbackCoroutine;

    public static ItemManager Instance { get; private set; }

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
        shopPanel.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Key") && !GameManager.Instance.hasKey)
        {
            GameManager.Instance.hasKey = true;
            collision.gameObject.SetActive(false);
        }

        if (collision.CompareTag("Coin"))
        {
            MoneyManager.Instance.IncreaseMoney(value);
            Destroy(collision.gameObject);

            // Mostrar feedback "+1" por 1 segundo
            ShowFeedback("+1", 1f);
        }

        if (collision.CompareTag("SpecialCardPickup"))
        {
            // Añadir carta especial y obtener su tipo
            SpecialCardInventory.Instance.AddRandomSpecialCard();
            SpecialCardType cardType = SpecialCardInventory.Instance.GetLatestCardType();

            Destroy(collision.gameObject);

            // Mostrar feedback con el tipo de carta por 2 segundos
            ShowFeedback($"Conseguiste carta especial: {GetCardTypeName(cardType)}", 2f);
        }

        if (collision.CompareTag("Shop"))
        {
            shopPanel.SetActive(true);
        }
     
        if (collision.CompareTag("Flag"))
        {
            LevelWarp.Instance.InitiateLevelWarp();
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Shop"))
        {
            shopPanel.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Door") && GameManager.Instance.hasKey)
        {
            GameManager.Instance.hasKey = false;
            string doorID = collision.gameObject.name;
            GameData.Instance.RegisterOpenedDoor(doorID);
            collision.gameObject.SetActive(false);
        }
    }

    private void ShowFeedback(string message, float duration)
    {
        // Detener el feedback anterior si está activo
        if (feedbackCoroutine != null)
        {
            StopCoroutine(feedbackCoroutine);
        }

        // Mostrar el nuevo mensaje
        pickupFeedbackText.text = message;
        feedbackCoroutine = StartCoroutine(ClearFeedbackAfterDelay(duration));
    }

    private IEnumerator ClearFeedbackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        pickupFeedbackText.text = "";
        feedbackCoroutine = null;
    }

    public string GetCardTypeName(SpecialCardType type)
    {
        // Traducir el tipo de carta a un nombre legible
        switch (type)
        {
            case SpecialCardType.RedWildCard: return "Comodin rojo";
            case SpecialCardType.GreenWildCard: return "Comodin Verde";
            case SpecialCardType.PinkWildCard: return "Comodin Rosa";
            case SpecialCardType.WhiteWildCard: return "Comodin Blanca";
            case SpecialCardType.Protector: return "Protectora";
            case SpecialCardType.DoubleAttack: return "Doble Ataque";
            case SpecialCardType.Healer: return "Sanadora";
            case SpecialCardType.Lens: return "Lupa";
            case SpecialCardType.WildCard: return "Comodin Trickster";
            default: return "Especial";
        }
    }
}