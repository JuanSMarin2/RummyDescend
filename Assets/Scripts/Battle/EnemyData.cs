using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemies/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public int health = 10;

    [Tooltip("Cada combinación representa un posible ataque del enemigo.")]
    public List<CardCombination> attackCombinations;
}

[System.Serializable]
public class CardCombination
{
    [Tooltip("Puedes agregar de 1 a 3 cartas por combinación.")]
    public List<Card> cards;
}
