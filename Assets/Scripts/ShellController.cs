using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour {
    // The name Unity gives shells instantiated at runtime
    public const string SHELL_CLONE = "Shell(Clone)";

    // Is the player in this shell?
    public bool posessed = false;

    private GameObject player;
    private Quaternion originalRotation;

    private void Start() {
        player = GameObject.FindWithTag("Player");
        originalRotation = transform.rotation;
    }

    public void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            player.GetComponent<PlayerController>().Posess(gameObject);
        }
    }

    public void Rotate(Quaternion rotation) {
        transform.rotation = originalRotation * rotation;
    }
}
