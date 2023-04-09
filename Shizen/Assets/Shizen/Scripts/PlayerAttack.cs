using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerAnimatorController pAnimController;
    [SerializeField] Element element;
    [SerializeField] bool breakComboOnTriggerExit = false;
    [SerializeField] bool stickToGround;
    [SerializeField] float maxGroundRange = 5f;
    [SerializeField] float groundOffset = 1f;
    [SerializeField] float detectionOffset = 1f;
    [SerializeField] LayerMask IsGround;
    [SerializeField] Transform raycastStart;
    [SerializeField] Transform GroundObject;
    private TargetSystem TargSyst;
    private bool rotate = false;
    private float rotateSpeed = 4;
    private Transform targetRotationObj;
    private Quaternion frozenTargetRot;
    private Animator animator;

    private bool resetOnDestroy = false;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        pAnimController = FindObjectOfType<PlayerAnimatorController>();
        targetRotationObj = FindObjectOfType<FollowCamera>().PlayerModel;
        TargSyst = FindObjectOfType<TargetSystem>();

        if (stickToGround)
        {
            RaycastHit hit;
            if (!Physics.Raycast(raycastStart.position, Vector3.down, out hit, maxGroundRange, IsGround))
            {
                // pAnimController.RemoveFromList(element, animator);
                resetOnDestroy = true;
                Destroying(0);
            }
            else
            {
                GroundObject.position = new Vector3(GroundObject.position.x, hit.point.y + groundOffset, GroundObject.position.z);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rotate)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, frozenTargetRot, Time.unscaledDeltaTime * rotateSpeed);
            if (transform.rotation == frozenTargetRot)
                rotate = false;
        }

        RaycastHit hit;
        if (stickToGround && Physics.Raycast(raycastStart.position, Vector3.down, out hit, maxGroundRange, IsGround))
        {
            if (hit.distance < groundOffset + detectionOffset)
            {
                GroundObject.position = new Vector3(GroundObject.position.x, hit.point.y + groundOffset, GroundObject.position.z);
            }
        }
    }

    private IEnumerator Destroying(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        // pAnimController.RemoveFromList(element, animator);
        Destroy(gameObject);
    }

    public void SetRotationEqualPlayer(float rotateSpd)
    {
        rotate = true;
        rotateSpeed = rotateSpd;
        frozenTargetRot = targetRotationObj.rotation;
    }

    public void AimToEnemyTarg(float rotateSpd)
    {
        rotate = true;
        rotateSpeed = rotateSpd;
        if (TargSyst.Target != null)
        {
            var targpos = TargSyst.Target.transform.position - transform.position;
            var targRot = Quaternion.LookRotation(targpos, transform.up);
            frozenTargetRot = targRot;
        }
        else { frozenTargetRot = targetRotationObj.rotation; }
    }

    private void OnTriggerExit(Collider other)
    {
        if (breakComboOnTriggerExit && other.TryGetComponent(out PlayerSkills skill))
        {
            skill.EndOfComboReset();
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }
    private void OnDestroy()
    {
        pAnimController.RemoveFromList(element, animator, resetOnDestroy);
    }

    public void SpawnPrefab(GameObject prefab)
    {
        var inst = Instantiate(prefab, transform);
        inst.transform.parent = null;
    }
}
