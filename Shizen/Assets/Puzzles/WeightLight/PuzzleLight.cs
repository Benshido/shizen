using UnityEngine;

public class PuzzleLight : MonoBehaviour
{    private Animator anim;
    [SerializeField] bool isOn = false;
    public bool IsOn { get { return isOn; } }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Switch()
    {
        isOn = !isOn;
        anim.SetBool("IsOn", isOn);
    }
}
