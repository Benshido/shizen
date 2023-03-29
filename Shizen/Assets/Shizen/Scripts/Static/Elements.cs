using System.Collections.Generic;

public enum Element
{
    Earth,
}

public static class Unlockables
{
    public static Dictionary<string, float> Elements = new()
    {
        {Element.Earth.ToString(), 0},
    };
}
