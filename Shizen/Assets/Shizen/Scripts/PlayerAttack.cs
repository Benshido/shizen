using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerAnimatorController pAnimController;
    [SerializeField] Elements element;
    [SerializeField] bool hasSetTarget = false;
    [SerializeField] bool breakComboOnTriggerExit = false;

    [SerializeField] bool stickToGround;
    [SerializeField] float maxGroundRange = 5f;
    [SerializeField] float groundOffset = 1f;
    [SerializeField] float detectionOffset = 1f;
    [SerializeField] LayerMask IsGround;
    [SerializeField] Transform raycastStart;
    [SerializeField] Transform GroundObject;

    [SerializeField] bool DestroyOnHitWall = false;
    [SerializeField] Transform raycastToWallStart;
    [SerializeField] float raycastToWallLength;
    private TargetSystem TargSyst;
    private bool rotate = false;
    private float rotateSpeed = 4;
    private Transform targetRotationObj;
    private Quaternion frozenTargetRot;
    private Animator animator;
    private GameObject targObj = null;
    private bool resetOnDestroy = false;

    private Vector3 lastPosition = Vector3.zero;

    // Start is called before the first frame update
    void Awake()
    {
        lastPosition = transform.position;
        animator = GetComponent<Animator>();
        pAnimController = FindObjectOfType<PlayerAnimatorController>();
        targetRotationObj = FindObjectOfType<FollowCamera>().PlayerModel;
        TargSyst = FindObjectOfType<TargetSystem>();
        if (TargSyst != null) targObj = TargSyst.Target;
        if (stickToGround)
        {
            RaycastHit hit;
            if (!Physics.Raycast(raycastStart.position, Vector3.down, out hit, maxGroundRange + detectionOffset, IsGround))
            {
                //if on a slope and higher ground is on spawn it will also check if that is in range
                var higher = raycastStart.position;
                higher.y += detectionOffset;
                if (Physics.Raycast(higher, Vector3.down, out hit, maxGroundRange + detectionOffset))
                {
                    GroundObject.position = new Vector3(GroundObject.position.x, hit.point.y + groundOffset, GroundObject.position.z);
                }
                else
                {
                    resetOnDestroy = true;
                    Destroy(gameObject);
                }
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

        if (hasSetTarget && targObj != null)
        {
            var targpos = targObj.transform.position - transform.position;
            var targRot = Quaternion.LookRotation(targpos, Vector3.up);

            transform.rotation = Quaternion.Lerp(transform.rotation, targRot, Time.unscaledDeltaTime * 30);
        }
        if (DestroyOnHitWall && raycastToWallStart != null)
        {
            RaycastHit hitWall;
            if (Physics.Raycast(raycastToWallStart.position, transform.forward, out hitWall, IsGround) ||
                Physics.Raycast(lastPosition, raycastToWallStart.position - lastPosition, out hitWall, Vector3.Distance(raycastToWallStart.position, lastPosition), IsGround))
            {
                if (hitWall.distance <= raycastToWallLength && !hitWall.collider.isTrigger)
                {
                    Destroy(gameObject);
                }
            }
            lastPosition = raycastToWallStart.position;
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
        Destroy(gameObject);
    }

    public void SetRotationEqualPlayer(float rotateSpd)
    {
        rotate = true;
        rotateSpeed = rotateSpd;
        frozenTargetRot = targetRotationObj.rotation;
    }

    public void SetRotationEqualCameraAim()
    {
        rotate = false;
        if (TargSyst.AimTarget != Vector3.zero) transform.rotation = Quaternion.LookRotation(TargSyst.AimTarget);
        else { rotate = true; frozenTargetRot = Camera.main.transform.rotation; }
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
