using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyCardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberText;
    [SerializeField] private Image backgroundImage;

    public void Setup(Card card)
    {
        if (numberText != null)
            numberText.text = card.number.ToString();

        if (backgroundImage != null)
            backgroundImage.color = GetColorBySuit(card.suit);
    }

    private Color GetColorBySuit(Suit suit)
    {
        switch (suit)
        {
            case Suit.Red: return Color.red;
            case Suit.Pink: return new Color(1f, 0.5f, 0.7f);
            case Suit.Green: return Color.green;
            case Suit.White: return Color.white;
            default: return Color.gray;
        }
    }
}
