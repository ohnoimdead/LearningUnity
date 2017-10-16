using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatcherController : MonoBehaviour {
    public float surveySpeed = 10.0f;

    private bool surveying = false;
    private Camera eye;
    private Plane[] eyeFrustum;
    private GameObject[] watchables;

    // Variables for determining if a watchable is seen
    private Vector3 rayDirection;

    private List<GameObject> seenObjects;

    public void Start() {
        eye = transform.Find("Camera").gameObject.GetComponent<Camera>();
        seenObjects = new List<GameObject>();
        StartCoroutine("Survey");
    }

    private void DealWithSeenObjects() {
        surveying = true;
        eyeFrustum = GeometryUtility.CalculateFrustumPlanes(eye);

        watchables = GameObject.FindGameObjectsWithTag("Watchable");

        foreach (GameObject watchable in watchables) {
            // First see if it's in the frustum
            if (GeometryUtility.TestPlanesAABB(eyeFrustum, watchable.GetComponent<Collider>().bounds)) {
                // Now see if we can see the top of the object's collider
                if (IsObjectSeen(watchable)) {
                    seenObjects.Add(watchable);

                    HealthManager hitHealthManager = watchable.GetComponent<HealthManager>();
                    if (hitHealthManager) {
                        hitHealthManager.ISeeYou();
                        surveying = false; // Stop here until all things are destroyed
                    }
                }
            }
        }
    }

    private void ZapSeenObjects() {
        HealthManager seenHealthManager;
        foreach (GameObject seenObject in seenObjects) {
            if (seenObject == null) { return; }
            seenHealthManager = seenObject.GetComponent<HealthManager>();
            if (seenHealthManager.Zap()) {
                // Now that the object has been destroyed, find the tile below this object and give it a gem if empty
                GameObject tile = TileBeneath(seenObject);
                if (tile) {
                    tile.GetComponent<ObjectStackingManager>().AddGem();
                }
            }
        }
    }

    // Can the eye see the top of the object?
    private bool IsObjectSeen(GameObject watchable) {
        RaycastHit hitResult;
        rayDirection = GetTopOfObject(watchable) - eye.transform.position;
        Physics.Raycast(eye.transform.position, rayDirection, out hitResult);
        if (hitResult.transform.gameObject.tag == "Watchable") {
            return true;
        }
        return false;
    }

    private GameObject TileBeneath(GameObject watchable) {
        RaycastHit hitResult;
        Physics.Raycast(GetTopOfObject(watchable), Vector3.down, out hitResult);
        if (hitResult.transform.gameObject.GetComponent<TileController>()) {
            return hitResult.transform.gameObject;
        }
        return null;
    }

    private Vector3 GetTopOfObject(GameObject obj) {
        return new Vector3(
            obj.transform.position.x,
            obj.transform.position.y + obj.GetComponent<Collider>().bounds.size.y,
            obj.transform.position.z);
    }

    private IEnumerator Survey() {
        while (true) {
            if (surveying) {
                transform.Rotate(0, 60.0f, 0);
                GetComponent<AudioSource>().Play();
            } else {
                surveying = true;
            }

            DealWithSeenObjects();

            yield return new WaitForSeconds(surveySpeed);

            ZapSeenObjects();
            seenObjects = new List<GameObject>();
        }
    }
}
