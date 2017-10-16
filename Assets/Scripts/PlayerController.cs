using System.Collections;
using UnityEngine;

/* All things player. Keep in mind that the player is really just a set of eyeballs.
 * The physical representation of the player is the shell that the player currently
 * possesses. The PlayerController keeps track of the players own energy vs using
 * HealthManager. */
public class PlayerController : MouseLookBehavior {
    public const string SEEN_MESSAGE = "HIDE!!";

    public float heightOfPlayersEyes = 1.62f;
    public GameObject startingTile;

    public int currentEnergy = 5;
    public float clearSeenMessageTime = 5.0f;

    [HideInInspector]
    public GameObject currentShell;

    private string seenMessage = "";
    private GUIStyle style;

    public override void Start() {
        base.Start();

        // Create the initial shell
        currentShell = Instantiate(sceneController.shellPrefab, startingTile.transform.position, Quaternion.identity).gameObject;
        currentShell.name = SceneController.SHELL;
        startingTile.GetComponent<ObjectStackingManager>().objectOnTop = currentShell;
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
        style = GUI.skin.GetStyle("Box");
        style.fontSize = 18;

        GUI.Box(new Rect(10, 10, 100, 28), "Energy: " + currentEnergy);
        GUI.Box(new Rect(120, 10, 100, 28), seenMessage);
    }

    public bool BuildShell() {
        if (currentEnergy > SceneController.SHELL_COST) {
            currentEnergy -= SceneController.SHELL_COST;
            return true;
        }
        return false;
    }

    public bool BuildPlatform() {
        if (currentEnergy > SceneController.PLATFORM_COST) {
            currentEnergy -= SceneController.PLATFORM_COST;
            return true;
        }
        return false;
    }

    public void AbsorbObject(GameObject obj) {
        if (obj.GetComponent<HealthManager>()) {
            currentEnergy += obj.GetComponent<HealthManager>().health;
        }
    }

    public void ISeeYou() {
        seenMessage = SEEN_MESSAGE;
        StartCoroutine(ClearSeenMessage());
    }

    public void Zap() {
        currentEnergy -= 1;

        if (currentEnergy <= 0) {
            sceneController.LoseGame();
        }
    }

    // Take posession of shell when clicked on
    public void Posess(GameObject shell) {
        // Swap the current shell and set posession
        currentShell.GetComponent<ShellController>().posessed = false;
        currentShell = shell;
        currentShell.GetComponent<ShellController>().posessed = true;

        // Move the player to the new shell's position and rotation
        transform.position = new Vector3(shell.transform.position.x, shell.transform.position.y + heightOfPlayersEyes, shell.transform.position.z);
        transform.rotation = shell.transform.rotation;

        // Make look seemless after posession
        ResetLook(shell.GetComponent<ShellController>().horizontalRotation, shell.GetComponent<ShellController>().verticalRotation);
    }

    private IEnumerator ClearSeenMessage() {
        yield return new WaitForSeconds(clearSeenMessageTime);
        seenMessage = "";
    }
}
