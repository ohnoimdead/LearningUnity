using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    // Mouse look stuff
    public float lookSensitivity = 5f;
    public int lookFrameBuffer = 10;
    public float lookUpMaxAngle = -60f;
    public float lookDownMaxAngle = 60f;

    // Gameplay stuff
    public GameObject myTile;
    public int currentEnergy = 100;
    public int teleportCost = 10;

    private Quaternion originalShellRotation, originalCameraRotation;
    private float rotationX, rotationY;
    private List<float> rotArrayX = new List<float>();
    private List<float> rotArrayY = new List<float>();
    private Camera childCamera;

    void Start() {
        childCamera = GetComponentInChildren<Camera>();

        if (childCamera) {
            originalCameraRotation = childCamera.transform.localRotation;
        }

        originalShellRotation = transform.localRotation;
    }

    void Update() {
        if (Input.GetMouseButton(1)) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            mouseLook();
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

    // Teleport the player when they click on a new tile. This gets called by the tile
    // clicked on.
    public void Teleport(GameObject newTile) {
        if (currentEnergy >= teleportCost) {
            // Don't allow the player to teleport to the tile they are currently on
            // because that's silly.
            if (myTile.transform.position != newTile.transform.position) {
                currentEnergy -= teleportCost;
                myTile = newTile;
                transform.position = newTile.transform.position;
            }
        }
    }

    private void mouseLook() {
        // Get the raw mouse input
        rotationX += Input.GetAxis("Mouse X") * lookSensitivity;
        rotationY += Input.GetAxis("Mouse Y") * lookSensitivity;

        // Calculate quats from the clamped average rotation for each axis
        Quaternion xQuat = Quaternion.AngleAxis(
            ClampAngle(GetAvgRot(rotationX, rotArrayX), -360f, 360f),
            Vector3.up);
        Quaternion yQuat = Quaternion.AngleAxis(
            ClampAngle(GetAvgRot(rotationY, rotArrayY), lookDownMaxAngle, lookUpMaxAngle),
            Vector3.left);

        // Rotate the camera and the shell on x-axis
        transform.localRotation = originalShellRotation * xQuat;

        // Rotate only the camera on the y-axis
        if (childCamera) {
            childCamera.transform.localRotation = originalCameraRotation * yQuat;
        }
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
