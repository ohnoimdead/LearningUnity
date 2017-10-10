using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MouseLookBehavior {
    public float heightOfPlayersEyes = 1.62f;

    // Gameplay stuff
    public GameObject currentTile;
    public GameObject currentShell;
    public int currentEnergy = 5;
    public int shellCost = 3;

    // References to prefabs
    public Transform shellPrefab;

    public override void Start() {
        base.Start();

        // Create the initial shell
        currentShell = Instantiate(shellPrefab, currentTile.transform.position, Quaternion.identity).gameObject;
        currentTile.GetComponent<TileController>().objectOnTop = currentShell;
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

    public GameObject BuildShell(GameObject tile) {
        if (currentEnergy >= shellCost) {
            currentEnergy -= shellCost;
            return Instantiate(shellPrefab, tile.transform.position, Quaternion.identity).gameObject;
        }
        return null;
    }

    public void AbsorbedShell() {
        currentEnergy += shellCost;
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

