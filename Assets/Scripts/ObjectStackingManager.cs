using UnityEngine;

public class ObjectStackingManager : MonoBehaviour {
    [HideInInspector]
    public GameObject objectOnTop;
    private PlayerController playerController;

    public void Start() {
        var player = GameObject.FindWithTag("Player");
        if (player) {
            playerController = player.GetComponent<PlayerController>();
        }
    }

    public void OnMouseOver() {
        // Create a new platform
        if (Input.GetKeyDown(KeyCode.Q)) {
            if (!objectOnTop) {
                objectOnTop = playerController.BuildPlatform(gameObject);
            }
        }

        // Create a new shell
        if (Input.GetKeyDown(KeyCode.W)) {
            if (!objectOnTop) {
                objectOnTop = playerController.BuildShell(gameObject);
            }
        }
    }

    public void RemoveObjectOnTop() {
        Destroy(objectOnTop);
        objectOnTop = null;
    }
}
