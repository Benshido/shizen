using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LightPuzzleCheck : MonoBehaviour
{
    [SerializeField] List<LightResult> lights = new();
    private bool solved = false;
    public bool Solved { get { return solved; } }
    public UnityEvent myEvent;

    public void UpdateResult()
    {
        for (int i = 0; i < lights.Count; i++)
        {
            if (lights[i].light.IsOn != lights[i].requiredResult) return;
            if (i == lights.Count - 1)
            {
                solved = true;
                myEvent.Invoke();
            }
        }
    }
}
