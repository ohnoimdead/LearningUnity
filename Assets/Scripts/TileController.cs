using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {
    public int bumpSteps = 30;
    public float bumpAmount = 0.005f;
    public float bumpWait = 0.005f;

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
        if (Input.GetMouseButtonDown(0)) {
            playerController.Teleport(gameObject);
        }
    }

    private IEnumerator BumpColor() {
        for (int steps = 0; steps < bumpSteps; steps++) {
            rend.material.color += colorBump;
            yield return new WaitForSeconds(bumpWait);
        }
    }
}
