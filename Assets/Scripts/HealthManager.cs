using UnityEngine;

public class HealthManager : MonoBehaviour {
    [HideInInspector]
    public int health;

    public void Start() {
        switch (gameObject.name) {
            case SceneController.SHELL_CLONE:
                health = SceneController.SHELL_COST;
                break;
            case SceneController.PLATFORM_CLONE:
                health = SceneController.PLATFORM_COST;
                break;
        }
    }

    // Called by the watcher when it sees this object
    public bool ISeeYou() {
        // If we see the shell the player currently posesses, zap the player
        if (gameObject.name == SceneController.SHELL_CLONE && gameObject.GetComponent<ShellController>().posessed) {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ISeeYou();
            return false;
        } else {
            health -= 1;

            if (health <= 0) {
                GameObject.Destroy(gameObject);
                return true;
            }

            return false;
        }
    }
}
