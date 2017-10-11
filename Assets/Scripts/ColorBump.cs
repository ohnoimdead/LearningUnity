using System.Collections;
using UnityEngine;

public class ColorBump : MonoBehaviour {
    public int bumpSteps = 10;
    public float bumpAmount = 0.05f;
    public float bumpWait = 0.01f;

    private Renderer rend;
    private Color originalColor;
    private Color colorBump;

    public void Start () {
        colorBump = new Color(bumpAmount, bumpAmount, bumpAmount);
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
	}

    public void OnMouseEnter() {
        StartCoroutine("BumpColor");
    }

    public void OnMouseExit() {
        StopCoroutine("BumpColor");
        rend.material.color = originalColor;
    }

    private IEnumerator BumpColor() {
        for (int steps = 0; steps < bumpSteps; steps++) {
            rend.material.color += colorBump;
            yield return new WaitForSeconds(bumpWait);
        }
    }
}
