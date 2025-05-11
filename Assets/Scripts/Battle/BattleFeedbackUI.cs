using UnityEngine;
using TMPro;
using System.Collections;

public class BattleFeedbackUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerActionText;
    [SerializeField] private TextMeshProUGUI enemyActionText;

    private Coroutine hidePlayerTextCoroutine;
    private Coroutine hideEnemyTextCoroutine;

    public static BattleFeedbackUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ShowPlayerAction(string message)
    {
        if (playerActionText != null)
        {
            playerActionText.text = message;
            if (hidePlayerTextCoroutine != null)
            {
                StopCoroutine(hidePlayerTextCoroutine);
            }
            hidePlayerTextCoroutine = StartCoroutine(HideTextAfterDelay(playerActionText, 3f));
        }
    }

    public void ShowEnemyAction(string message)
    {
        if (enemyActionText != null)
        {
            enemyActionText.text = message;
            if (hideEnemyTextCoroutine != null)
            {
                StopCoroutine(hideEnemyTextCoroutine);
            }
            hideEnemyTextCoroutine = StartCoroutine(HideTextAfterDelay(enemyActionText, 3f));
        }
    }

    private IEnumerator HideTextAfterDelay(TextMeshProUGUI textElement, float delay)
    {
        yield return new WaitForSeconds(delay);
        textElement.text = "";
    }
}