using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    private bool iFrame = false;
    private float iFrameTimer = 0;
    private float iFrameDuration = 0.1f;

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
        if (epSlider != null) epSlider.value = ep;

        if (iFrame)
        {
            iFrameTimer += Time.unscaledDeltaTime;
            if (iFrameTimer >= iFrameDuration)
            {
                iFrameTimer = 0;
                iFrame = false;
            }
        }

        if (!IsAlive)
        {
            StartCoroutine(Death());
        }
    }

    public void TakeDamage(float damage)
    {
        if (!IsAlive || iFrame) return;

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

    public void IFrame(float seconds)
    {
        iFrameDuration = seconds;
        iFrameTimer = 0;
        iFrame = true;
    }

    public IEnumerator Death()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        StopAllCoroutines();
    }
}
