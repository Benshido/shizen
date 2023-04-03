using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] Transform target = null;
    [SerializeField] Transform playerModel = null;
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
            player.transform.Rotate(0, addToX, 0);

            if (!player.IsMoving)
            {
                //Keep the player model facing the same direction by rotating it the oposite direction
                playerModel.Rotate(0, -addToX, 0);


            }
            else
            {
                //When dashing or running the rotation should match the expectations of the player
                var dashLookDir = player.DashMovement;
                if (player.BackStep) dashLookDir = -dashLookDir;
                var Y = Quaternion.LookRotation(player.transform.TransformDirection(player.Movement + dashLookDir));
                if (!player.IsRunning && !player.IsDashing) Y = Quaternion.LookRotation(playerModel.transform.forward);

                playerModel.rotation = Quaternion.RotateTowards(playerModel.rotation, Y, 1 * Time.unscaledDeltaTime * 800);
            }

            
            //ZOOMING
            targetZoom += Input.mouseScrollDelta.y / zoomSpeedDivider;
            if (Mathf.Abs(targetZoom) > Mathf.Abs(cameraMaxDistance)) targetZoom = cameraMaxDistance;
            if (Mathf.Abs(targetZoom) < Mathf.Abs(cameraMinDistance)) targetZoom = cameraMinDistance;

            transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, 0, targetZoom), Time.unscaledDeltaTime * 8);
       
            //If combo changes and there is a target, get rotating
            if(pSkills.ComboCount != lastComboCount && targSyst.Target != null)
            {
                rotateToTarget = true;
                lastComboCount = pSkills.ComboCount;
            }

            if (rotateToTarget)
            {
                //look at enemy when attacking
                Quaternion y = Quaternion.LookRotation(targSyst.Target.transform.position - transform.position);
                playerModel.rotation = Quaternion.RotateTowards(playerModel.rotation, y, 10 * Time.unscaledDeltaTime * 800);
               
                targRotTimer += Time.unscaledDeltaTime;
                if (targRotTimer >= targRotDuration)
                {
                    targRotTimer = 0;
                    rotateToTarget = false;
                }
            }
        }
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