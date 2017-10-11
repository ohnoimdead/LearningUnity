using UnityEngine;

public class PlayerController : MouseLookBehavior {
    public float heightOfPlayersEyes = 1.62f;
    public GameObject startingTile;

    // Gameplay stuff
    public int currentEnergy = 5;
    public int shellCost = 3;
    public int platformCost = 2;

    [HideInInspector]
    public GameObject currentShell;

    public override void Start() {
        base.Start();

        // Create the initial shell
        currentShell = Instantiate(sceneController.shellPrefab, startingTile.transform.position, Quaternion.identity).gameObject;
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
        GUI.Box(new Rect(10, 10, 100, 24), "Energy: " + currentEnergy);
    }

    public bool BuildShell() {
        if (currentEnergy >= shellCost) {
            currentEnergy -= shellCost;
            return true;
        }
        return false;
    }

    public bool BuildPlatform() {
        if (currentEnergy >= platformCost) {
            currentEnergy -= platformCost;
            return true;
        }
        return false;
    }

    public void AbsorbedShell() {
        currentEnergy += shellCost;
    }

    public void AbsorbedPlatform() {
        currentEnergy += platformCost;
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
}
