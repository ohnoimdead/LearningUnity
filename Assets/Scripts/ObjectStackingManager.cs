using UnityEngine;

/* Encapsulates the behavior of having the ability to know about an object
 * stacked on top of this one. Currently applies to game tiles and platforms
 * as those are the only objects that currently allow other stuff on top of
 * them. */
public class ObjectStackingManager : MonoBehaviour {
    public GameObject objectOnTop;

    private SceneController sceneController;
    private PlayerController playerController;

    public void Start() {
        sceneController = GameObject.FindGameObjectWithTag("SceneController").GetComponent<SceneController>();

        var player = GameObject.FindWithTag("Player");
        if (player) {
            playerController = player.GetComponent<PlayerController>();
        }
    }

    public void OnMouseOver() {
        if (sceneController.gameState == SceneController.GameState.Playing) {
            // Create a new platform
            if (Input.GetKeyDown(KeyCode.Q)) {
                if (!objectOnTop) {
                    if (playerController.BuildPlatform()) {
                        objectOnTop = Instantiate(sceneController.platformPrefab, TopOfSelf(), Quaternion.identity).gameObject;
                        objectOnTop.name = SceneController.PLATFORM;
                    }
                }
            }

            // Create a new shell
            if (Input.GetKeyDown(KeyCode.W)) {
                if (!objectOnTop) {
                    if (playerController.BuildShell()) {
                        objectOnTop = Instantiate(sceneController.shellPrefab, TopOfSelf(), Quaternion.identity).gameObject;
                        objectOnTop.name = SceneController.SHELL;
                    }
                }
            }
        }
    }

    public bool HasObjectOnTop() {
        return objectOnTop != null;
    }

    public void AddGem() {
        if (objectOnTop && objectOnTop.name == SceneController.GEM) {
            return;
        }
        objectOnTop = Instantiate(sceneController.gemPrefab, TopOfSelf(), Quaternion.identity).gameObject;
        objectOnTop.name = SceneController.GEM;
    }

    // Get the point on top of the current object by using the box collider's size
    private Vector3 TopOfSelf() {
        return new Vector3(
            transform.position.x,
            GetComponent<BoxCollider>().bounds.size.y + transform.position.y + 0.01f,
            transform.position.z);
    }
}
