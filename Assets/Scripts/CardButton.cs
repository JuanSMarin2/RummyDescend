using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CardButton : MonoBehaviour
{
    private Card cardData;
    private Transform originalParent;
    private Button button;


    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.RemoveAllListeners(); // Limpia cualquier suscripción previa
        button.onClick.AddListener(OnClick);


    }

    public void Setup(Card card, Transform originalLayout)
    {
        cardData = card;
        originalParent = originalLayout;
    }

    public void OnClick()
    {


            // --- REFLECTAJE ---
            if (BattleManager.Instance != null && BattleManager.Instance.IsReflectAvailable())
            {
                Card lastEnemyCard = BattleManager.Instance.GetLastEnemyCard();

                bool validBySuit = cardData.suit == lastEnemyCard.suit && cardData.number == lastEnemyCard.number + 1;
                bool validByNumber = cardData.number == lastEnemyCard.number && cardData.suit != lastEnemyCard.suit;

                if (validBySuit || validByNumber)
                {
                    ReflectWithCard(cardData);


                    return;
                }
        }
        else
        {
            if (cardData == null)
            {
                Debug.LogError("cardData es null. ¿Olvidaste llamar a Setup()?");
                return;
            }

            Debug.Log("Clic en carta: " + cardData.number + " " + cardData.suit);

            if (CardSequenceManager.Instance == null)
            {
                Debug.LogError("CardSequenceManager.Instance es null");
                return;
            }

            // --- Modo normal (ataque del jugador) ---
            bool placed = CardSequenceManager.Instance.TryPlaceCard(gameObject, cardData);
        }


     
   


       

    }


    private void ReflectWithCard(Card reflectCard)
    {

        Debug.Log("¡Ataque reflejado con: " + reflectCard.number + " " + reflectCard.suit + "!");

        var enemyAttack = BattleManager.Instance.GetCurrentEnemyAttack();
        int enemyAttackSum = 0;
        foreach (Card card in enemyAttack)
            enemyAttackSum += card.number;

        int totalReflectDamage = Mathf.CeilToInt((enemyAttackSum + reflectCard.number) / 2f);

        Debug.Log($"Daño reflejado al enemigo: {totalReflectDamage}");

        BattleManager.Instance.ShowTemporaryPlayerMessage($"Daño reflejado al enemigo: {totalReflectDamage}");

        // Remueve la carta del inventario
        CardInventory.Instance.RemoveCard(reflectCard);

       

       



        // 💥 También destruye su representación visual en pantalla
        

        // Finaliza el turno del enemigo
    

        BattleManager.Instance.EndEnemyTurnAfterReflect(totalReflectDamage);

        StartCoroutine(RevertAfterReflect());
       


    }




    public void ReturnToOriginal()
    {
        transform.SetParent(originalParent, false);
    }

    public Card GetCardData()
    {
        return cardData;
    }


    public IEnumerator RevertAfterReflect()
    {

        Debug.Log("Revirtiendo sequencia");
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Revirtiendo sequencia 2");

        CardSequenceManager.Instance.RevertSequence();

       
        Destroy(gameObject);

    }

}
