using UnityEngine;

public class ShellController : MouseLookBehavior {
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
