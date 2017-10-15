using UnityEngine;

public class SceneController : MonoBehaviour {
    public enum GameState { Starting, Playing, Lose, Win };

    public const string SHELL = "Shell";
    public const string PLATFORM = "Platform";
    public const string GEM = "Gem";

    public const int SHELL_COST = 3;
    public const int PLATFORM_COST = 2;
    public const int GEM_COST = 1;

    public GameState gameState;

    [Header("MouseLook")]
    public float lookSensitivity = 5f;
    public int lookFrameBuffer = 10;
    public float lookUpMaxAngle = 60f;
    public float lookDownMaxAngle = -60f;

    [Header("Prefabs")]
    public Transform shellPrefab;
    public Transform platformPrefab;
    public Transform gemPrefab;
}
