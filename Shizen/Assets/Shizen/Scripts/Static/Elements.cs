using System.Collections.Generic;

public enum Element
{
    Earth,
    Water,
    Air,
    Fire,
}

public static class Unlockables
{
    public readonly static Dictionary<string, float> Elements = new()
    {
        {Element.Earth.ToString(), 1},
        {Element.Water.ToString(), 1},
        {Element.Air.ToString(), 0},
        {Element.Fire.ToString(), 0},
    };
}
