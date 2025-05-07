public enum Suit { Red, Pink, Green, White }

[System.Serializable]
public class Card
{
    public int number; // 1 to 10
    public Suit suit;

    public Card(int number, Suit suit)
    {
        this.number = number;
        this.suit = suit;
    }

    public override string ToString()
    {
        return $"{number} of {suit}";
    }
}