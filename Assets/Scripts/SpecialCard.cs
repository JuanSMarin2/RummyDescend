[System.Serializable]
public enum SpecialCardType
{
    RedWildCard,
    GreenWildCard,
    PinkWildCard,
    WhiteWildCard,
    Protector,
    DoubleAttack,
    Healer,
    WildCard,
    Lens
}

[System.Serializable]
public class SpecialCard
{
    public SpecialCardType type;

    public SpecialCard(SpecialCardType type)
    {
        this.type = type;
    }

    public override string ToString()
    {
        return type.ToString();
    }
}