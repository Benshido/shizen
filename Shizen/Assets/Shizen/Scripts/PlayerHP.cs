using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    public float MaxHP { get { return maxHP; } }
    [SerializeField] float maxHP = 100;

    public float HP { get { return hp; } }
    [SerializeField] float hp = 100;

    public bool IsAlive { get { if (hp > 0) return true; return false; } }

    [SerializeField] Slider hpSlider;

    void Start()
    {
        hp = maxHP;

        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = hp;
        }
    }

    public void TakeDamage(float damage)
    {
        if (!IsAlive) return;

        hp -= damage;
        if (hpSlider != null) hpSlider.value = hp;
    }
}
