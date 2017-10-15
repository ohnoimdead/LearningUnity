using UnityEngine;

public class LevelStartController : MonoBehaviour {
    private SceneController sceneController;
    private PlayerController playerController;
    private float originalTimeScale;

    public void Start() {
        sceneController = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        originalTimeScale = Time.timeScale;
        Time.timeScale = 0;
    }

    public void OnGUI() {
        if (!sceneController.playing) {
            if (GUI.Button(new Rect(
                (Screen.width / 2) - 50,
                (Screen.height - 100),
                100,
                50
                ), "START")) {
                sceneController.playing = true;
                Time.timeScale = originalTimeScale;
                GetComponentInChildren<Camera>().enabled = false;
                playerController.GetComponent<Camera>().enabled = true;
            }
        }
    }
}
