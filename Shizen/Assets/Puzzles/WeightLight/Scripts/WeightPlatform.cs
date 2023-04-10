using System.Collections.Generic;
using UnityEngine;

public class WeightPlatform : MonoBehaviour
{
    public bool IsPressed { get { return isPressed; } }
    private bool isPressed = false;

    [SerializeField] List<PuzzleLight> lights = new();
    private LightPuzzleCheck checker;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        checker = GetComponentInParent<LightPuzzleCheck>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPressed = true;
            animator.SetBool("IsPressed", isPressed);
            for (int i = 0; i < lights.Count; i++) lights[i].Switch();
            checker.UpdateResult();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPressed = false;
            animator.SetBool("IsPressed", isPressed);
        }
    }

}
