using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemies/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int health = 10;
    public int maxHealth = 10;

    [Tooltip("Cada combinación representa un posible ataque del enemigo.")]
    public List<CardCombination> attackCombinations;

    [Tooltip("Cartas en el inventario del enemigo.  Usadas para reflejar.")]
    public List<Card> enemyInventory; // Nueva lista para el inventario del enemigo

    public void ResetHealth()
    {
        health = maxHealth;
    }
}

[System.Serializable]
public class CardCombination
{
    [Tooltip("Puedes agregar de 1 a 3 cartas por combinación.")]
    public List<Card> cards;
}
