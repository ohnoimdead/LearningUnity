﻿using UnityEngine;

/* Handles basic UI elements to do everything from show a level's name
 * and start button to showing the player loose or player win UIs.
 * Super simple. Don't judge. */
public class OverlayController : MonoBehaviour {
    public string levelName = "Level";

    private SceneController sceneController;
    private int panelWidth = 400;
    private int panelHeight = 50;
    private GUIStyle style;
    private string levelWinMessage = "You solved my Watcher puzzle!";
    private string loseMessage = "You ran out of energy!";
    private string gameWinMessage = "You win everything!";

    public void Start() {
        sceneController = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();
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
        DrawCenteredOverlay(levelName);

        if (GUI.Button(new Rect(
            (Screen.width / 2) - 50,
            (Screen.height - 100),
            100,
            50
            ), "START")) {
            sceneController.StartLevel();
            GetComponentInChildren<Camera>().enabled = false;
        }
    }

    private void DrawWinOverlay() {
        if (sceneController.AreScenesLeft()) {
            DrawCenteredOverlay(levelWinMessage);

            if (GUI.Button(new Rect(
                (Screen.width / 2) - 50,
                (Screen.height - 100),
                100,
                50
                ), "NEXT")) {
                sceneController.NextScene();
            }
        } else {
            DrawCenteredOverlay(gameWinMessage);

            if (GUI.Button(new Rect(
                (Screen.width / 2) - 50,
                (Screen.height - 100),
                100,
                50
                ), "QUIT")) {
                sceneController.StopGame();
            }
        }
    }

    private void DrawLoseOverlay() {
        DrawCenteredOverlay(loseMessage);

        if (GUI.Button(new Rect(
            (Screen.width / 2) - 50,
            (Screen.height - 100),
            100,
            50
            ), "RETRY")) {
            sceneController.RestartScene();
        }
    }

    private void DrawCenteredOverlay(string message) {
        GUI.Box(new Rect(
            (Screen.width / 2) - (panelWidth / 2),
            (Screen.height / 2) - (panelHeight / 2),
            panelWidth,
            panelHeight), message);
    }
}
