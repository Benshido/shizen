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
    [SerializeField] LayerMask IsGround;
    [SerializeField] Transform raycastStart;
    [SerializeField] Transform GroundObject;
    private TargetSystem TargSyst;
    private bool rotate = false;
    private float rotateSpeed = 4;
    private Transform targetRotationObj;
    private Quaternion frozenTargetRot;
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        pAnimController = FindObjectOfType<PlayerAnimatorController>();
        targetRotationObj = FindObjectOfType<FollowCamera>().PlayerModel;
        TargSyst = FindObjectOfType<TargetSystem>();

        RaycastHit hit;
        if (!Physics.Raycast(raycastStart.position, Vector3.down, out hit, maxGroundRange, IsGround))
        {
            // pAnimController.RemoveFromList(element, animator);
            Destroy(gameObject);
        }
        else
        {
            GroundObject.position = new Vector3(GroundObject.position.x, hit.point.y + groundOffset, GroundObject.position.z);
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
        if (Physics.Raycast(raycastStart.position, Vector3.down, out hit, maxGroundRange, IsGround))
        {
            if (hit.distance < groundOffset + 0.5f)
            {
                GroundObject.position = new Vector3(GroundObject.position.x, hit.point.y + groundOffset, GroundObject.position.z);
            }
        }
    }

    private IEnumerator Destroying()
    {
        yield return new WaitForSecondsRealtime(10);
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
            Debug.Log(frozenTargetRot);
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
        pAnimController.RemoveFromList(element, animator);
    }

    public void SpawnPrefab(GameObject prefab)
    {
        var inst = Instantiate(prefab, transform);
        inst.transform.parent = null;
    }
}
