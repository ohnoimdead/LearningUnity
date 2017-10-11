using UnityEngine;

public class ObjectStackingManager : MonoBehaviour {
    [HideInInspector]
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
        // Create a new platform
        if (Input.GetKeyDown(KeyCode.Q)) {
            if (!objectOnTop) {
                if(playerController.BuildPlatform()) {
                    objectOnTop = Instantiate(sceneController.platformPrefab, transform.position, Quaternion.identity).gameObject;
                }
            }
        }

        // Create a new shell
        if (Input.GetKeyDown(KeyCode.W)) {
            if (!objectOnTop) {
                if (playerController.BuildShell()) {
                    objectOnTop = Instantiate(sceneController.shellPrefab, transform.position, Quaternion.identity).gameObject;
                }
            }
        }
    }

    // Called when the player absorbs an object
    public void RemoveObjectOnTop() {
        Destroy(objectOnTop);
        objectOnTop = null;
    }
}
