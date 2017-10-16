using UnityEngine;

/* Basic shell functionality. Thankfully due to behavior encapsulation this is
 * a fairly small amount of code for a fairly broad spectrum of behavior. */
public class ShellController : MouseLookBehavior {
    // Is the player in this shell?
    public bool posessed = false;

    private int health = SceneController.SHELL_COST;
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

    public void ISeeYou() {
        health -= 1;

        if (health <= 0) {
            GameObject.Destroy(gameObject);
        }
    }
}
