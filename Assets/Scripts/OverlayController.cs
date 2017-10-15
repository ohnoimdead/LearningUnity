using UnityEngine;
using UnityEngine.SceneManagement;

public class OverlayController : MonoBehaviour {
    public string levelName = "Level";

    private SceneController sceneController;
    private PlayerController playerController;
    private float originalTimeScale;
    private int panelWidth = 400;
    private int panelHeight = 50;
    private GUIStyle style;
    private string levelWinMessage = "You solved my Watcher puzzle!";
    private string loseMessage = "You ran out of energy!";
    private string gameWinMessage = "You win everything!";

    public void Start() {
        sceneController = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        originalTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    public void OnGUI() {
        // You can only call GUI functions from within OnGUI, however, it seems like a lot
        // of overhead to constantly reset the style on every draw. Is there a better way?
        style = GUI.skin.GetStyle("Box");
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = 18;

        switch (sceneController.gameState) {
            case SceneController.GameState.Starting:
                DrawStartingOverlay();
                break;
            case SceneController.GameState.Win:
                DrawWinOverlay();
                break;
            case SceneController.GameState.Lose:
                DrawLoseOverlay();
                break;
        }
    }

    private void DrawStartingOverlay() {
        DrawOverlay(levelName);

        if (GUI.Button(new Rect(
            (Screen.width / 2) - 50,
            (Screen.height - 100),
            100,
            50
            ), "START")) {
            sceneController.gameState = SceneController.GameState.Playing;
            Time.timeScale = originalTimeScale;
            GetComponentInChildren<Camera>().enabled = false;
            playerController.GetComponent<Camera>().enabled = true;
        }
    }

    private void DrawWinOverlay() {
        if (SceneManager.sceneCountInBuildSettings - 1 > SceneManager.GetActiveScene().buildIndex) {
            DrawOverlay(levelWinMessage);

            if (GUI.Button(new Rect(
                (Screen.width / 2) - 50,
                (Screen.height - 100),
                100,
                50
                ), "NEXT")) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        } else {
            DrawOverlay(gameWinMessage);

            if (GUI.Button(new Rect(
                (Screen.width / 2) - 50,
                (Screen.height - 100),
                100,
                50
                ), "QUIT")) {
                Application.Quit();
            }
        }
    }

    private void DrawLoseOverlay() {
        DrawOverlay(loseMessage);

        if (GUI.Button(new Rect(
            (Screen.width / 2) - 50,
            (Screen.height - 100),
            100,
            50
            ), "RETRY")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void DrawOverlay(string message) {
        GUI.Box(new Rect(
            (Screen.width / 2) - (panelWidth / 2),
            (Screen.height / 2) - (panelHeight / 2),
            panelWidth,
            panelHeight), message);
    }
}
