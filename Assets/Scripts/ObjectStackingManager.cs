using UnityEngine;

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
        // Create a new platform
        if (Input.GetKeyDown(KeyCode.Q)) {
            if (!objectOnTop) {
                if(playerController.BuildPlatform()) {
                    objectOnTop = Instantiate(sceneController.platformPrefab, TopOfSelf(), Quaternion.identity).gameObject;
                }
            }
        }

        // Create a new shell
        if (Input.GetKeyDown(KeyCode.W)) {
            if (!objectOnTop) {
                if (playerController.BuildShell()) {
                    objectOnTop = Instantiate(sceneController.shellPrefab, TopOfSelf(), Quaternion.identity).gameObject;
                }
            }
        }
    }

    public bool HasObjectOnTop() {
        return objectOnTop != null;
    }

    // Get the point on top of the current object by using the box collider's size
    private Vector3 TopOfSelf() {
        return new Vector3(
            transform.position.x,
            GetComponent<BoxCollider>().bounds.size.y + transform.position.y,
            transform.position.z);
    }
}
