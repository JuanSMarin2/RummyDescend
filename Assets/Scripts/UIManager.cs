using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] public TextMeshProUGUI m_CoinText;

    [SerializeField] private GameObject UIKey;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_CoinText != null)
        {
            m_CoinText.text = MoneyManager.Instance.coins.ToString();
        }

        if (GameManager.Instance.hasKey)
            UIKey.gameObject.SetActive(true);

        if (!GameManager.Instance.hasKey)
            UIKey.gameObject.SetActive(false);
    }


}
