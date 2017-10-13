using System.Collections;
using UnityEngine;

public class HealthManager : MonoBehaviour {
    public int hitIndicatorSteps = 10;
    public float hitIndicatorDelay = 0.01f;

    [HideInInspector]
    public int health;

    private Renderer rend;
    private Color hitIndicatorColor = new Color(0.1f, -0.1f, -0.1f);
    private Color originalColor;
    private bool pulsing = false;

    public void Start() {
        switch (gameObject.name) {
            case SceneController.SHELL:
                health = SceneController.SHELL_COST;
                break;
            case SceneController.PLATFORM:
                health = SceneController.PLATFORM_COST;
                break;
            case SceneController.GEM:
                health = SceneController.GEM_COST;
                break;
        }

        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    // Called by the watcher when it sees this object
    public bool ISeeYou() {
        if (!pulsing) {
            StartCoroutine(HitFlash());
        }

        // If we see the shell the player currently posesses, zap the player
        if (gameObject.name == SceneController.SHELL && gameObject.GetComponent<ShellController>().posessed) {
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

    private IEnumerator HitFlash() {
        pulsing = true;
        int steps = 0;
        while (steps < hitIndicatorSteps) {
            rend.material.color += hitIndicatorColor;
            steps++;
            yield return new WaitForSeconds(hitIndicatorDelay);
        }
        while (steps > 0) {
            rend.material.color -= hitIndicatorColor;
            steps--;
            yield return new WaitForSeconds(hitIndicatorDelay);
        }
        rend.material.color = originalColor;
        pulsing = false;
    }
}
