using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform target = null;
    [SerializeField] LayerMask ignore;
    private Transform cam;
    [SerializeField] Transform playerModel = null;
    [SerializeField] Transform camParent = null;
    [SerializeField] TargetSystem targSyst = null;
    public Transform PlayerModel { get { return playerModel; } }
    [SerializeField] PlayerMovement player = null;
    [SerializeField] PlayerSkills pSkills = null;

    [SerializeField] float tiltUpLim;
    [SerializeField] float tiltLowLim;

    private float pitchRotation;
    [SerializeField] float targetZoom = -5;
    [SerializeField] float camSpeedMultiplier = 1f;
    [SerializeField] float zoomSpeedDivider = 5f;
    [SerializeField] float cameraMinDistance = -3;
    [SerializeField] float cameraMaxDistance = -15;

    private float targRotDuration = 0.1f;
    private float targRotTimer = 0f;
    private bool rotateToTarget = false;
    private int lastComboCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        CursorLock();
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //quick temporary hotkeys for inverting axis
        if (Input.GetKeyDown(KeyCode.Y)) GeneralSettings.InvertMouseY = !GeneralSettings.InvertMouseY;
        if (Input.GetKeyDown(KeyCode.X)) GeneralSettings.InvertMouseX = !GeneralSettings.InvertMouseX;

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            CursorUnlock();
        }
        else
        {
            CursorLock();
            //MOUSE LOOK
            if (GeneralSettings.InvertMouseY) pitchRotation += Input.GetAxis("Mouse Y") * GeneralSettings.CameraYSpeed * camSpeedMultiplier;
            else pitchRotation -= Input.GetAxis("Mouse Y") * GeneralSettings.CameraYSpeed * camSpeedMultiplier;

            float addToX = Input.GetAxis("Mouse X") * GeneralSettings.CameraXSpeed * camSpeedMultiplier;
            if (GeneralSettings.InvertMouseX) addToX = -addToX;

            //apply camera pitch limits
            if (pitchRotation > tiltUpLim) pitchRotation = tiltUpLim;
            else if (pitchRotation < tiltLowLim) pitchRotation = tiltLowLim;

            //adjust pitch of the camera
            target.localEulerAngles = new Vector3(pitchRotation, 0, 0);

            //rotate player to face away from the camera
            camParent.transform.Rotate(0, addToX, 0);
            camParent.position = Vector3.Lerp(camParent.position, player.transform.position, Time.unscaledDeltaTime * 15);

            if (player.IsMoving)
            {
                //When dashing or running the rotation should match the expectations of the player
                var dashLookDir = player.DashMovement;
                if (player.BackStep) dashLookDir = -dashLookDir;
                var Y = Quaternion.LookRotation(camParent.transform.TransformDirection(player.Movement + dashLookDir));
                if (!player.IsRunning && !player.IsDashing) Y = Quaternion.LookRotation(player.transform.forward);

                if (player.Can_Move) player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, Y, 1 * Time.unscaledDeltaTime * 800);
            }


            //ZOOMING
            targetZoom += Input.mouseScrollDelta.y / zoomSpeedDivider;
            if (Mathf.Abs(targetZoom) > Mathf.Abs(cameraMaxDistance)) targetZoom = cameraMaxDistance;
            if (Mathf.Abs(targetZoom) < Mathf.Abs(cameraMinDistance)) targetZoom = cameraMinDistance;

            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, 0, targetZoom), Time.unscaledDeltaTime * 8);

            //If combo changes and there is a target, get rotating
            if (pSkills.ComboCount != lastComboCount && targSyst.Target != null)
            {
                if (pSkills.ComboCount > 0) rotateToTarget = true;
                lastComboCount = pSkills.ComboCount;
            }

            if (rotateToTarget)
            {
                targRotTimer += Time.unscaledDeltaTime;
                if (targSyst.Target == null) { targRotTimer = 0; rotateToTarget = false; }
                if (targRotTimer >= targRotDuration)
                {
                    targRotTimer = 0;
                    rotateToTarget = false;
                }
                else
                {
                    //look at enemy when attacking
                    var targ = targSyst.Target.transform.position;
                    targ.y = player.transform.position.y;
                    Quaternion y = Quaternion.LookRotation(targ - player.transform.position);
                    player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, y, 10 * Time.unscaledDeltaTime * 800);
                }
            }
        }

        var camNewPos = transform.position;

        if (Physics.Raycast(target.position, transform.position - target.position, out RaycastHit hit, Mathf.Abs(transform.localPosition.z), ignore))
        {
            camNewPos = hit.point;
        }

        cam.position = camNewPos;
        camNewPos = cam.localPosition;
        camNewPos += new Vector3(0, 0.05f, 0.1f);
        cam.localPosition = camNewPos;
    }

    private void CursorLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void CursorUnlock()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}