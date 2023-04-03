using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TargetSystem : MonoBehaviour
{
    [SerializeField] KeyCode LockOn;
    private bool lockOn = false;
    [SerializeField] float targetMaxAngle = 30;
    [Header("Enemies Within range")]
    [SerializeField] bool inRangeOutline = true;
    [SerializeField] float inRangeOutlineWidth = 1.5f;
    [SerializeField] Color inRangeOutlineColor = Color.red;

    [Header("Enemy Selected")]
    [SerializeField] float targetedOutlineWidth = 3.5f;
    [SerializeField] Color targetedOutlineColor = Color.red;

    [Header("Enemy LockedOn")]
    [SerializeField] float LockedOutlineWidth = 3.5f;
    [SerializeField] Color LockedOutlineColor = Color.white;
    public List<GameObject> Targets
    {
        get { return targets; }
    }
    private List<GameObject> targets = new();
    public GameObject Target
    {
        get { return target; }
    }
    private GameObject target;

    private bool alteredLockonLine = false;

    private void Update()
    {
        var deadOnes = targets.Where(x => x == null || !x.GetComponent<EnemyHP>().IsAlive).ToList();
        for (int i = 0; i < deadOnes.Count; i++) { targets.Remove(deadOnes[i]); lockOn = false; }

        if (targets.Count > 0)
        {
            var smallestAngle = targetMaxAngle;
            if (!lockOn)
            {
                List<GameObject> sortedTargList = targets.OrderBy(objTransf =>
                {
                    var dir = objTransf.transform.position - Camera.main.transform.position;
                    var camFw = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
                    var targDir2D = new Vector3(dir.x, dir.y, dir.z);
                    float angle = Vector3.Angle(camFw, targDir2D);

                    //set outline back to basic
                    var outline = objTransf.GetComponent<Outline>();
                    outline.OutlineWidth = inRangeOutlineWidth;
                    outline.OutlineColor = inRangeOutlineColor;

                    if (angle < smallestAngle) smallestAngle = angle;

                    return angle;

                }).ToList();
                //set targeted outline if angle is small enough
                if (smallestAngle < targetMaxAngle)
                {
                    target = sortedTargList[0];
                    var outline1 = target.GetComponent<Outline>();
                    outline1.OutlineWidth = targetedOutlineWidth;
                    outline1.OutlineColor = targetedOutlineColor;
                }
                else target = null;

                targets = sortedTargList;
            }
            else if (!alteredLockonLine && target != null)
            {
                alteredLockonLine = true;
                var outline = target.GetComponent<Outline>();
                outline.OutlineWidth = LockedOutlineWidth;
                outline.OutlineColor = LockedOutlineColor;
            }
        }

        if (Input.GetKeyDown(LockOn))
        {
            lockOn = !lockOn;
            alteredLockonLine = false;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<EnemyHP>())
        {

            targets.Add(other.gameObject);
            var outline = other.GetComponent<Outline>();
            if (!outline)
            {
                outline = other.AddComponent<Outline>();
            }

            outline.OutlineWidth = inRangeOutlineWidth;
            outline.OutlineColor = inRangeOutlineColor;
            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.enabled = inRangeOutline;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<EnemyHP>())
        {
            targets.Remove(other.gameObject);

            var outline = other.GetComponent<Outline>();
            outline.enabled = false;
        }
    }
}
