using UnityEngine;
using UnityEngine.SceneManagement;

/* Contains some scene-wide constants as well as scene transition management.
 * There must be one of these in the scene for anything else to work. */
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

    private float originalTimeScale;
    private PlayerController playerController;

    public void Start() {
        originalTimeScale = Time.timeScale;
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Time.timeScale = 0; // Everything starts stopped - OverlayController calls StartLevel below
    }

    public void StartLevel() {
            gameState = SceneController.GameState.Playing;
            Time.timeScale = originalTimeScale;
            playerController.GetComponent<Camera>().enabled = true;
    }

    public bool AreScenesLeft() {
        return SceneManager.sceneCountInBuildSettings - 1 > SceneManager.GetActiveScene().buildIndex;
    }

    public void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = originalTimeScale;
    }

    public void NextScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = originalTimeScale;
    }

    public void LoseGame() {
        gameState = SceneController.GameState.Lose;
        Time.timeScale = 0;
    }

    public void StopGame() {
        Application.Quit();
    }
}
