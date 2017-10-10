using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MouseLookBehavior {
    // The name Unity gives shells instantiated at runtime
    public const string SHELL_CLONE = "Shell(Clone)";

    // Is the player in this shell?
    public bool posessed = false;

    private GameObject player;

    public override void Start() {
        base.Start();

        onlyRotateX = true;
        player = GameObject.FindWithTag("Player");
    }

    public void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            player.GetComponent<PlayerController>().Posess(gameObject);
        }
    }

    public void Update() {
        if (Input.GetMouseButton(1) && posessed) {
            MouseLook();
        }
    }
}
