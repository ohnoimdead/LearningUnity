using UnityEngine;

public class SceneController : MonoBehaviour {
    public const string SHELL_CLONE = "Shell(Clone)";
    public const string PLATFORM_CLONE = "Platform(Clone)";

    [Header("MouseLook")]
    public float lookSensitivity = 5f;
    public int lookFrameBuffer = 10;
    public float lookUpMaxAngle = 60f;
    public float lookDownMaxAngle = -60f;

    [Header("Prefabs")]
    public Transform shellPrefab;
    public Transform platformPrefab;
}
