using TMPro;
using UnityEngine;
using System.Collections;

public class Shop : MonoBehaviour
{

    public TextMeshProUGUI shopFeedbackText;
    public int fullHealCost = 5;
    public int specialCardCost = 3;
    public int newInventoryCost = 3;
    public int singleCardCost = 1;

    private void Start()
    {

        if (shopFeedbackText == null)
        {
            Debug.LogError("shopFeedbackText no está asignado en la Tienda!");
        }
    }

    // Producto 1: Cura total
    public void BuyFullHeal()
    {
        if (GameManager.Instance.playerHealth < 50)
        {
            if (MoneyManager.Instance.coins >= fullHealCost)
            {
                MoneyManager.Instance.RemoveCoins(fullHealCost);
                GameManager.Instance.playerHealth = 50;
                PlayerController.Instance.UpdatePlayerHealthText();
                shopFeedbackText.text = "¡Vida restaurada al máximo!";
                StartCoroutine(ClearFeedbackText());
            }
            else
            {
                shopFeedbackText.text = "No tienes suficientes monedas.";
                StartCoroutine(ClearFeedbackText());
            }
        }
        else
        {
            shopFeedbackText.text = "¡Ya tienes la vida al máximo!";
            StartCoroutine(ClearFeedbackText());
        }
    }

    // Producto 2: Carta especial
    public void BuySpecialCard()
    {
        if (SpecialCardInventory.Instance.cards.Count < SpecialCardInventory.Instance.MaxCards) //verifica si el inventario no esta lleno
        {
            if (MoneyManager.Instance.coins >= specialCardCost)
            {
                MoneyManager.Instance.RemoveCoins(specialCardCost);
                SpecialCardInventory.Instance.AddRandomSpecialCard();
                SpecialCardType cardType = SpecialCardInventory.Instance.GetLatestCardType();

                shopFeedbackText.text = $"Conseguiste carta especial: {ItemManager.Instance.GetCardTypeName(cardType)}";
                StartCoroutine(ClearFeedbackText());
            }
            else
            {
                shopFeedbackText.text = "No tienes suficientes monedas.";
                StartCoroutine(ClearFeedbackText());
            }
        }
        else
        {
            shopFeedbackText.text = "¡Inventario de cartas especiales lleno!";
            StartCoroutine(ClearFeedbackText());
        }
    }
    public void BuyNewInventory()
    {
        if (MoneyManager.Instance.coins >= newInventoryCost)
        {
            MoneyManager.Instance.RemoveCoins(newInventoryCost);
            CardInventory.Instance.ClearInventory();
            for (int i = 0; i < CardInventory.Instance.maxCards; i++)
            {
                CardInventory.Instance.AddRandomCard();
            }
        
            shopFeedbackText.text = "¡Inventario de cartas renovado!";
            StartCoroutine(ClearFeedbackText());
        }
        else
        {
            shopFeedbackText.text = "No tienes suficientes monedas.";
            StartCoroutine(ClearFeedbackText());
        }
    }

    // Producto 4: Carta roja
    public void BuyRedCard()
    {
        if (CardInventory.Instance.cards.Count < CardInventory.Instance.maxCards)
        {
            if (MoneyManager.Instance.coins >= singleCardCost)
            {
                MoneyManager.Instance.RemoveCoins(singleCardCost);
                CardInventory.Instance.AddCard(new Card(Random.Range(1, 11), Suit.Red));
       
                shopFeedbackText.text = "¡Carta roja añadida!";
                StartCoroutine(ClearFeedbackText());
            }
            else
            {
                shopFeedbackText.text = "No tienes suficientes monedas.";
                StartCoroutine(ClearFeedbackText());
            }
        }
        else
        {
            shopFeedbackText.text = "¡Inventario de cartas lleno!";
            StartCoroutine(ClearFeedbackText());
        }
    }

    // Producto 5: Carta verde
    public void BuyGreenCard()
    {
        if (CardInventory.Instance.cards.Count < CardInventory.Instance.maxCards)
        {
            if (MoneyManager.Instance.coins >= singleCardCost)
            {
                MoneyManager.Instance.RemoveCoins(singleCardCost);
                CardInventory.Instance.AddCard(new Card(Random.Range(1, 11), Suit.Green));
        
                shopFeedbackText.text = "¡Carta verde añadida!";
                StartCoroutine(ClearFeedbackText());
            }
            else
            {
                shopFeedbackText.text = "No tienes suficientes monedas.";
                StartCoroutine(ClearFeedbackText());
            }
        }
        else
        {
            shopFeedbackText.text = "¡Inventario de cartas lleno!";
            StartCoroutine(ClearFeedbackText());
        }
    }

    // Producto 6: Carta rosa
    public void BuyPinkCard()
    {
        if (CardInventory.Instance.cards.Count < CardInventory.Instance.maxCards)
        {
            if (MoneyManager.Instance.coins >= singleCardCost)
            {
                MoneyManager.Instance.RemoveCoins(singleCardCost);
                CardInventory.Instance.AddCard(new Card(Random.Range(1, 11), Suit.Pink));
            
                shopFeedbackText.text = "¡Carta rosa añadida!";
                StartCoroutine(ClearFeedbackText());
            }
            else
            {
                shopFeedbackText.text = "No tienes suficientes monedas.";
                StartCoroutine(ClearFeedbackText());
            }
        }
        else
        {
            shopFeedbackText.text = "¡Inventario de cartas lleno!";
            StartCoroutine(ClearFeedbackText());
        }
    }

    // Producto 7: Carta blanca
    public void BuyWhiteCard()
    {
        if (CardInventory.Instance.cards.Count < CardInventory.Instance.maxCards)
        {
            if (MoneyManager.Instance.coins >= singleCardCost)
            {
                MoneyManager.Instance.RemoveCoins(singleCardCost);
                CardInventory.Instance.AddCard(new Card(Random.Range(1, 11), Suit.White));
        
                shopFeedbackText.text = "¡Carta blanca añadida!";
                StartCoroutine(ClearFeedbackText());
            }
            else
            {
                shopFeedbackText.text = "No tienes suficientes monedas.";
                StartCoroutine(ClearFeedbackText());
            }
        }
        else
        {
            shopFeedbackText.text = "¡Inventario de cartas lleno!";
            StartCoroutine(ClearFeedbackText());
        }
    }


    // Corrutina para limpiar el texto de feedback después de un segundo
    private IEnumerator ClearFeedbackText()
    {
        yield return new WaitForSeconds(1f);
        shopFeedbackText.text = "";
    }
}