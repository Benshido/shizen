using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target = null;
    public Transform playerModel = null;
    public PlayerMovement player = null;

    public float tiltUpLim;
    public float tiltLowLim;

    private float pitchRotation;
    private int camSpeedMultiplier = 60;

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
            if (GeneralSettings.InvertMouseY) pitchRotation += Input.GetAxis("Mouse Y") * GeneralSettings.CameraYSpeed * Time.unscaledDeltaTime * camSpeedMultiplier;
            else pitchRotation -= Input.GetAxis("Mouse Y") * GeneralSettings.CameraYSpeed * Time.unscaledDeltaTime * camSpeedMultiplier;

            float addToX = Input.GetAxis("Mouse X") * GeneralSettings.CameraXSpeed * Time.unscaledDeltaTime * camSpeedMultiplier;
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