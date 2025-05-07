using UnityEngine;

public static class CurrentLevel
{
    private static int level = 0;

    public static int Level => level;

    public static void IncreaseLevel()
    {
        level++;
        Debug.Log($"Nivel aumentado a: {level}");
    }

    public static string GetNextScene()
    {
        switch (level)
        {
            case 1: return "LVL1";
            case 2: return "LVL2";
            case 3: return "LVL3";
            case 4: return "Credits";
            default: return "MainMenu"; // Caso por defecto
        }
    }

    public static void ResetLevels()
    {
        level = 0;
    }
}