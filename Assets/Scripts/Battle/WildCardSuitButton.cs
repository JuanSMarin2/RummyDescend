using UnityEngine;
using UnityEngine.UI;

public class WildCardSuitButton : MonoBehaviour
{
    [SerializeField] private Suit assignedSuit;
    [SerializeField] private GameObject wildCardToSuitPanel;
    [SerializeField] private GameObject wildCardToNumberPanel;

    private bool firstClick = true;

    public void OnSuitButtonClick()
    {
        if (firstClick)
        {
            if (wildCardToSuitPanel != null)
            {
                wildCardToSuitPanel.SetActive(false);
            }

            if (wildCardToNumberPanel != null)
            {
                wildCardToNumberPanel.SetActive(true);
                WildCardNumberButton[] numberButtons = wildCardToNumberPanel.GetComponentsInChildren<WildCardNumberButton>();
                foreach (var button in numberButtons)
                {
                    button.SetWildCardSuit(assignedSuit);
                }
            }

            firstClick = false;
        }
        else
        {
            firstClick = true;
        }
    }

    // Puedes mantener esta variable pública para configurarla en el Editor
    public Suit GetAssignedSuit()
    {
        return assignedSuit;
    }
}