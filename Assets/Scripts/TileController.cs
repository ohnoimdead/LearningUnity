using UnityEngine;

/* Handles absorbing stuff stacked on the tile, also the ability to understand
 * what's at the top of the pile for cases of stacked platforms. */
public class TileController : MonoBehaviour {
    private PlayerController playerController;
    private GameObject objectOnTop;
    private SceneController sceneController;

	void Start () {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        sceneController = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();
	}

    public void OnMouseOver() {
        if (sceneController.gameState == SceneController.GameState.Playing) {
            // Absorb whatever is on the tile, starting at the top
            if (Input.GetKeyDown(KeyCode.Space)) {
                objectOnTop = RecurseToTop(gameObject);
                if (objectOnTop && objectOnTop != gameObject) {
                    if (objectOnTop.name == SceneController.SHELL && objectOnTop.GetComponent<ShellController>().posessed) {
                        return;
                    }
                    playerController.AbsorbObject(objectOnTop);
                    if (objectOnTop.GetComponent<WatcherController>()) {
                        Time.timeScale = 0;
                        sceneController.gameState = SceneController.GameState.Win;
                    }
                    GameObject.Destroy(objectOnTop);
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
