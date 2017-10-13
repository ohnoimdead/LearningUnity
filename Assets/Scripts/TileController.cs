using UnityEngine;

public class TileController : MonoBehaviour {
    private PlayerController playerController;
    private GameObject objectOnTop;

	void Start () {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}

    public void OnMouseOver() {
        // Absorb whatever is on the tile, starting at the top
        if (Input.GetKeyDown(KeyCode.Space)) {
            objectOnTop = RecurseToTop(gameObject);
            if(objectOnTop && objectOnTop != gameObject) {
                if (objectOnTop.name == SceneController.SHELL && objectOnTop.GetComponent<ShellController>().posessed) {
                    return;
                }
                playerController.AbsorbObject(objectOnTop);
                GameObject.Destroy(objectOnTop);
            }
        }
    }

    private GameObject RecurseToTop(GameObject currentObject) {
        ObjectStackingManager sm = currentObject.GetComponent<ObjectStackingManager>();
        if (!(sm && sm.HasObjectOnTop())) {
            return currentObject;
        } else {
            return RecurseToTop(sm.objectOnTop);
        }
    }
}
