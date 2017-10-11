using System.Collections;
using UnityEngine;

public class TileController : MonoBehaviour {
    public int bumpSteps = 30;
    public float bumpAmount = 0.005f;
    public float bumpWait = 0.005f;
    public GameObject objectOnTop;

    private Renderer rend;
    private Color originalColor;
    private Color colorBump;
    private PlayerController playerController;

	void Start () {
        colorBump = new Color(bumpAmount, bumpAmount, bumpAmount);
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;

        // Just hang on to this ref for later
        var player = GameObject.FindWithTag("Player");
        if (player) {
            playerController = player.GetComponent<PlayerController>();
        }
	}

    public void OnMouseEnter() {
        StartCoroutine("BumpColor");
    }

    public void OnMouseExit() {
        StopCoroutine("BumpColor");
        rend.material.color = originalColor;
    }

    public void OnMouseOver() {
        // Absorb whatever is on the tile
        if (Input.GetKeyDown(KeyCode.Space)) {
            if(objectOnTop) {
                switch(objectOnTop.name) {
                    case ShellController.SHELL_CLONE:
                        // Basically prevent the player from absorbing the shell they are currently in
                        if (objectOnTop.GetComponent<ShellController>().posessed) {
                            break;
                        }
                        Destroy(objectOnTop);
                        objectOnTop = null;
                        playerController.AbsorbedShell();
                        break;
                    case PlatformController.PLATFORM_CLONE:
                        Destroy(objectOnTop);
                        objectOnTop = null;
                        playerController.AbsorbedPlatform();
                        break;
                }
            }
        }

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

    private IEnumerator BumpColor() {
        for (int steps = 0; steps < bumpSteps; steps++) {
            rend.material.color += colorBump;
            yield return new WaitForSeconds(bumpWait);
        }
    }
}
