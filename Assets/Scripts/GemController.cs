using UnityEngine;

public class GemController : MonoBehaviour {
    public float rotationSpeedMin = 10.0f;
    public float rotationSpeedMax = 20.0f;

    private float rotationSpeed;

    public void Start () {
        // pick a speed
        rotationSpeed = Random.Range(rotationSpeedMin, rotationSpeedMax);
    }

    public void Update() {
        gameObject.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
