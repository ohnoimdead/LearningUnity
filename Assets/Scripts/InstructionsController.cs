using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsController : MonoBehaviour {
    private int width = 600;
    private int height = 400;
    private string instructions = @"HERE'S THE DEAL:

You must clear the land of the evil WATCHER!
That's the big eye looking thing.
If it sees the shell you are possessing it will steal your energy.

To be rid of the Watcher, you must absorb it.
Absorb things by pointing at the square they are on and
hitting the space bar.

RMB - Look around
LMB - Possess shell
Q - Create a stackable step-stool (2 energy)
W - Create a shell that you can possess (3 energy)

Watch your energy level and good luck!";

    public void OnGUI() {
        var centeredStyle = GUI.skin.GetStyle("Box");
        centeredStyle.alignment = TextAnchor.MiddleCenter;
        centeredStyle.fontSize = 18;

        GUI.Box(new Rect(
            (Screen.width / 2) - (width / 2),
            (Screen.height / 2) - (height / 2),
            width,
            height), instructions, centeredStyle);

        if (GUI.Button(new Rect(
            (Screen.width / 2) - 50,
            (Screen.height - 100),
            100,
            50
            ), "START")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
