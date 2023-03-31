using System.Collections;
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
    private bool rotate = false;
    private float rotateSpeed = 4;
    private Transform targetRotationObj;
    private Vector3 frozenTargetRot;
    private Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        pAnimController = FindObjectOfType<PlayerAnimatorController>();
        targetRotationObj = FindObjectOfType<FollowCamera>().PlayerModel;
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
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, frozenTargetRot, Time.unscaledDeltaTime * rotateSpeed);
            if (transform.eulerAngles == frozenTargetRot)
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
        frozenTargetRot = targetRotationObj.eulerAngles;
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
}
