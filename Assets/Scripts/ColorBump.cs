using System.Collections;
using UnityEngine;

/* Encapsulates the behavior of highlighting when the player mouses over
 * the GameObject this is attached to.
 * NOTE: The GameObject must have a collider or the OnMouse events will not fire. */
public class ColorBump : MonoBehaviour {
    public int bumpSteps = 10;
    public float bumpAmount = 0.05f;
    public float bumpWait = 0.01f;

    private SceneController sceneController;
    private Renderer rend;
    private Color originalColor;
    private Color colorBump;
    private bool bumping = false;

    public void Start () {
        sceneController = GameObject.FindObjectOfType<SceneController>();
        colorBump = new Color(bumpAmount, bumpAmount, bumpAmount);
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
	}

    public void OnMouseEnter() {
        if (sceneController.gameState == SceneController.GameState.Playing) {
            if (!bumping) {
                StartCoroutine("BumpColor");
            }
        }
    }

    public void OnMouseExit() {
        if (sceneController.gameState == SceneController.GameState.Playing) {
            if (bumping) {
                StopCoroutine("BumpColor");
                bumping = false;
            }
            rend.material.color = originalColor;
        }
    }

    private IEnumerator BumpColor() {
        bumping = true;
        for (int steps = 0; steps < bumpSteps; steps++) {
            rend.material.color += colorBump;
            yield return new WaitForSeconds(bumpWait);
        }
        bumping = false;
    }
}
