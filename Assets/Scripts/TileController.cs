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
        //if (Input.GetMouseButtonDown(0)) {
        //    playerController.Teleport(gameObject);
        //}
        // Absorb whatever is on the tile
        if (Input.GetKeyDown(KeyCode.Space)) {
            if(objectOnTop) {
                switch(objectOnTop.name) {
                    case ShellController.SHELL_CLONE:
                        // Basically prevent the player from absorbing the shell they are currently in
                        if (objectOnTop.name == ShellController.SHELL_CLONE &&
                            objectOnTop.GetComponent<ShellController>().posessed) {
                            break;
                        }
                        Destroy(objectOnTop);
                        objectOnTop = null;
                        playerController.AbsorbedShell();
                        break;
                }
            }
        }

        // Create a new shell
        if (Input.GetKeyDown(KeyCode.W)) {
            if (!(objectOnTop && objectOnTop.name == ShellController.SHELL_CLONE)) {
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
