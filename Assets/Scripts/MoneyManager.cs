using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
   
        public int coins = 0;



    public static MoneyManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UpdateCoinText();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    public void IncreaseMoney(int amount)
    {
        coins += amount;

        
    }

    private void Update()
    {
        UpdateCoinText();
    }

    private void UpdateCoinText()
    {

    }
}