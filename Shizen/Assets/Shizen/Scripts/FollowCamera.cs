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

            if (pitchRotation > tiltUpLim) pitchRotation = tiltUpLim;
            else if (pitchRotation < tiltLowLim) pitchRotation = tiltLowLim;

            target.localEulerAngles = new Vector3(pitchRotation, 0, 0);

            if (GeneralSettings.InvertMouseX) player.transform.Rotate(0, -addToX, 0);
            else player.transform.Rotate(0, addToX, 0);

            if (!player.Moving)
            {
                if (GeneralSettings.InvertMouseX) playerModel.Rotate(0, addToX, 0);
                else playerModel.Rotate(0, -addToX, 0);
            }
            else
            {
                var Y = Vector3.RotateTowards(playerModel.forward, player.transform.forward, 1 * Time.unscaledDeltaTime * 15, 0f);
                playerModel.rotation = Quaternion.LookRotation(Y);
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