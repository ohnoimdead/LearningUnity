using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    // Mouse look stuff
    public float lookSensitivity = 5f;
    public int lookFrameBuffer = 10;
    public float lookUpMaxAngle = -60f;
    public float lookDownMaxAngle = 60f;
    public float heightOfPlayersEyes = 1.62f;
    private Quaternion originalRotation;
    private float horizontalRotation, verticalRotation;
    private List<float> rotArrayX = new List<float>();
    private List<float> rotArrayY = new List<float>();

    // Gameplay stuff
    public GameObject currentTile;
    public GameObject currentShell;
    public int currentEnergy = 5;
    public int createShellCost = 3;

    // References to prefabs
    public Transform shell;

    void Start() {
        originalRotation = transform.rotation;

        // Create the initial shell
        currentShell = Instantiate(shell, currentTile.transform.position, Quaternion.identity).gameObject;
        currentTile.GetComponent<TileController>().objectOnTop = currentShell;
        currentShell.GetComponent<ShellController>().posessed = true;
    }

    void Update() {
        if (Input.GetMouseButton(1)) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            MouseLook();
        } else {
            if (Cursor.visible == false) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    void OnGUI() {
        GUI.Box(new Rect(10, 10, 100, 24), "Energy: " + currentEnergy);
    }

    public GameObject BuildShell(GameObject tile) {
        if (currentEnergy >= createShellCost) {
            currentEnergy -= createShellCost;
            return Instantiate(shell, tile.transform.position, Quaternion.identity).gameObject;
        }
        return null;
    }

    public void AbsorbedShell() {
        currentEnergy += createShellCost;
    }

    public void Posess(GameObject shell) {
        currentShell.GetComponent<ShellController>().posessed = false;
        currentShell = shell;
        currentShell.GetComponent<ShellController>().posessed = true;
        transform.position = new Vector3(shell.transform.position.x, shell.transform.position.y + heightOfPlayersEyes, shell.transform.position.z);
        transform.rotation = shell.transform.rotation;
        // TODO: need the shell to track it's own rotation based on input axis, then copy the cumulative offsets over here on teleport
    }

    private void MouseLook() {
        // Get the raw mouse input
        horizontalRotation += Input.GetAxis("Mouse X") * lookSensitivity;
        verticalRotation += Input.GetAxis("Mouse Y") * lookSensitivity;
        horizontalRotation = ClampAngle(horizontalRotation, -360f, 360f);
        verticalRotation = ClampAngle(verticalRotation, lookDownMaxAngle, lookUpMaxAngle);

        // Calculate quats from the clamped average rotation for each axis
        Quaternion xQuat = Quaternion.AngleAxis(GetAvgRot(horizontalRotation, rotArrayX), Vector3.up);
        Quaternion yQuat = Quaternion.AngleAxis(GetAvgRot(verticalRotation, rotArrayY), Vector3.left);

        // Rotate the player
        transform.rotation = originalRotation * xQuat * yQuat;
        // Also rotate the current shell on x only
        currentShell.GetComponent<ShellController>().Rotate(xQuat);
    }

    // Returns the average of the rotations stored in the rotation frame buffer
    private float GetAvgRot(float rot, List<float> rotArray) {
        float rotAvg = 0f;

        rotArray.Add(rot);

        // If our frame buffer is beyond max size, pop old values off the front (FIFO queue)
        while (rotArray.Count > lookFrameBuffer) {
            rotArray.RemoveAt(0);
        }

        rotArray.ForEach(r => { rotAvg += r; });

        return rotAvg /= rotArray.Count;
    }

    // Deal with passing max rotation and also clamp values based on max.
    private float ClampAngle(float angle, float min, float max) {
        angle = angle % 360;

        if ((angle >= -360F) && (angle <= 360F)) {
            if (angle < -360F) {
                angle += 360F;
            }
            if (angle > 360F) {
                angle -= 360F;
            }
        }

        return Mathf.Clamp(angle, min, max);
    }
}
