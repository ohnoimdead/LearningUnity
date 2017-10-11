﻿using UnityEngine;
using System.Collections.Generic;

public class MouseLookBehavior : MonoBehaviour {
    public float lookSensitivity = 5f;
    public int lookFrameBuffer = 10;
    public float lookUpMaxAngle = 60f;
    public float lookDownMaxAngle = -60f;

    [HideInInspector]
    public Quaternion xQuat, yQuat;
    [HideInInspector]
    public bool onlyRotateX = false;
    [HideInInspector]
    public float horizontalRotation, verticalRotation;
    [HideInInspector]
    public List<float> rotArrayX = new List<float>();
    [HideInInspector]
    public List<float> rotArrayY = new List<float>();

    private Quaternion originalRotation;

    public virtual void Start() {
        originalRotation = transform.rotation;
    }

    public void MouseLook() {
        // Get the raw mouse input
        horizontalRotation += Input.GetAxis("Mouse X") * lookSensitivity;
        verticalRotation += Input.GetAxis("Mouse Y") * lookSensitivity;
        horizontalRotation = ClampAngle(horizontalRotation, -360f, 360f);
        verticalRotation = ClampAngle(verticalRotation, lookDownMaxAngle, lookUpMaxAngle);

        // Calculate quats from the clamped average rotation for each axis
        xQuat = Quaternion.AngleAxis(GetAvgRot(horizontalRotation, rotArrayX), Vector3.up);
        yQuat = Quaternion.AngleAxis(GetAvgRot(verticalRotation, rotArrayY), Vector3.left);

        // Rotate the player
        if (onlyRotateX) {
            transform.rotation = originalRotation * xQuat;
        } else {
            transform.rotation = originalRotation * xQuat * yQuat;
        }
    }

    public void ResetLook(float newHorizontalRotation, float newVerticalRotation) {
        // Reset the mouse look values so looking starts from the new perspective
        horizontalRotation = newHorizontalRotation;
        verticalRotation = newVerticalRotation;
        // Clear out the frame buffers
        rotArrayX = new List<float>();
        rotArrayY = new List<float>();
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