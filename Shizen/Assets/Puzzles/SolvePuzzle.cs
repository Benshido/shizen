using UnityEngine;

public class SolvePuzzle : MonoBehaviour
{
    public void Solved()
    {
        var anim = GetComponent<Animator>();
        if (anim != null) anim.SetBool("Solved", true);
    }
}
