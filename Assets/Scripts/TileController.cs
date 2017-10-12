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
        // Absorb whatever is on the tile, starting at the top
        if (Input.GetKeyDown(KeyCode.Space)) {
            objectOnTop = RecurseToTop(gameObject);
            if(objectOnTop) {
                switch(objectOnTop.name) {
                    case SceneController.SHELL_CLONE:
                        // Basically prevent the player from absorbing the shell they are currently in
                        if (objectOnTop.GetComponent<ShellController>().posessed) {
                            break;
                        }
                        playerController.AbsorbedShell();
                        GameObject.Destroy(objectOnTop);
                        break;
                    case SceneController.PLATFORM_CLONE:
                        playerController.AbsorbedPlatform();
                        GameObject.Destroy(objectOnTop);
                        break;
                    case SceneController.GEM:
                        playerController.AbsorbedGem();
                        GameObject.Destroy(objectOnTop);
                        break;
                }
            }
        }
    }

    private GameObject RecurseToTop(GameObject currentObject) {
        ObjectStackingManager sm = currentObject.GetComponent<ObjectStackingManager>();
        if (!(sm && sm.HasObjectOnTop())) {
            return currentObject;
        } else {
            return RecurseToTop(sm.objectOnTop);
        }
    }
}
