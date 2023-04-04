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

    public float MaxEP { get { return maxEP; } }
    [SerializeField] float maxEP = 100;

    public float EP { get { return ep; } }
    [SerializeField] float ep = 100;
    [SerializeField] float epRegen = 2;

    [SerializeField] Slider epSlider;

    void Start()
    {
        hp = maxHP;
        ep = maxEP;

        if (hpSlider != null)
        {
            hpSlider.maxValue = maxHP;
            hpSlider.value = hp;
        }
        if (epSlider != null)
        {
            epSlider.maxValue = maxEP;
            epSlider.value = ep;
        }
    }


    private void Update()
    {
        ep += epRegen * Time.unscaledDeltaTime;
        if (ep > maxEP) ep = maxEP;
        epSlider.value = ep;
    }

    public void TakeDamage(float damage)
    {
        if (!IsAlive) return;

        hp -= damage;
        if (hpSlider != null) hpSlider.value = hp;
    }

    public bool ComsumeStamina(float stamina)
    {
        if (!IsAlive || ep - stamina < 0) return false;
        ep -= stamina;
        if (epSlider != null) epSlider.value = ep;
        return true;
    }
}
