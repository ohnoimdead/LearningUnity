using UnityEngine;

public class TileController : MonoBehaviour {
    private PlayerController playerController;
    private GameObject objectOnTop;

	void Start () {
        var player = GameObject.FindWithTag("Player");
        if (player) {
            playerController = player.GetComponent<PlayerController>();
        }
	}

    public void OnMouseOver() {
        // Absorb whatever is on the tile
        if (Input.GetKeyDown(KeyCode.Space)) {
            objectOnTop = GetComponent<ObjectStackingManager>().objectOnTop;
            if(objectOnTop) {
                switch(objectOnTop.name) {
                    case SceneController.SHELL_CLONE:
                        // Basically prevent the player from absorbing the shell they are currently in
                        if (objectOnTop.GetComponent<ShellController>().posessed) {
                            break;
                        }
                        playerController.AbsorbedShell();
                        GetComponent<ObjectStackingManager>().RemoveObjectOnTop();
                        break;
                    case SceneController.PLATFORM_CLONE:
                        playerController.AbsorbedPlatform();
                        GetComponent<ObjectStackingManager>().RemoveObjectOnTop();
                        break;
                }
            }
        }
    }
}
