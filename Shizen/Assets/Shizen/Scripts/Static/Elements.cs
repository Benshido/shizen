using System.Collections.Generic;
using UnityEngine;

public enum Elements
{
    Earth,
    Water,
    Air,
    Fire,
}

public enum AttackMode
{
    Normal,
    Aim
}

public static class Unlockables
{
    public readonly static Dictionary<string, Element> Elements = new()
    {
        { global::Elements.Earth.ToString(), new Element(1,Color.green)},
        { global::Elements.Water.ToString(),new Element(1,Color.cyan, AttackMode.Aim)},
        { global::Elements.Air.ToString(), new Element(0,Color.gray)},
        { global::Elements.Fire.ToString(), new Element(0,Color.red, AttackMode.Aim)},
    };
}

public class Element
{
    public int Level { get { return level; } }
    private int level = 0;

    private readonly int[] expForLevel = new[]
    {
        1,
        10, 
        20, 
        30, 
        30, 
        30, 
        30,
        40,
        50,
        80,
    };

    public float Exp { get { return exp; } }
    private float exp = 0;

    public AttackMode HeavyAttackMode { get { return heavyAttackMode; } }
    private AttackMode heavyAttackMode = AttackMode.Normal;

    public Color Color { get { return color; } }
    private Color color = Color.white;

    public Element(int startLevel, Color color, AttackMode heavyAttackMode = AttackMode.Normal)
    {
        for (int i = 0; i < startLevel; i++)
        {
            AddExp(expForLevel[i]);
        }

        this.color = color;
        this.heavyAttackMode = heavyAttackMode;
    }

    public void AddExp(float addExp)
    {
        exp += addExp;
        var curExp = exp;
        for(int i = 0; i < expForLevel.Length; i++)
        {
            curExp-=expForLevel[i];
            if (i>=Level && curExp >= 0) level++;
        }
    }
}
