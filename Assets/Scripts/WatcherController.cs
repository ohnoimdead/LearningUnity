using System.Collections;
using UnityEngine;

public class WatcherController : MonoBehaviour {
    public float rotationSpeed = 10.0f;

    private bool surveying = false;
    private Camera eye;
    private Plane[] eyeFrustum;
    private GameObject[] watchables;

    // Variables for determining if a watchable is seen
    private Vector3 topOfWatchable;
    private Vector3 rayDirection;
    private RaycastHit objectHit;

    public void Start() {
        eye = transform.Find("Camera").gameObject.GetComponent<Camera>();
        StartCoroutine("Survey");
    }

    private void DealWithSeenObjects() {
        eyeFrustum = GeometryUtility.CalculateFrustumPlanes(eye);
        eyeFrustum[3] = new Plane(Vector3.up, 0);

        watchables = GameObject.FindGameObjectsWithTag("Watchable");

        foreach (GameObject watchable in watchables) {
            // First see what is in the frustum
            if (GeometryUtility.TestPlanesAABB(eyeFrustum, watchable.GetComponent<Collider>().bounds)) {
                // Now see if we can see the top of the object's head
                topOfWatchable = new Vector3(
                    watchable.transform.position.x,
                    watchable.transform.position.y + watchable.GetComponent<Collider>().bounds.size.y,
                    watchable.transform.position.z);
                rayDirection = topOfWatchable - eye.transform.position;
                if (Physics.Raycast(eye.transform.position, rayDirection, out objectHit)) {
                    Debug.Log(objectHit.transform.gameObject.name);
                }
            }
        }
    }

    private IEnumerator Survey() {
        while (true) {
            // Skip the first rotation so surveying doesn't start on the first frame
            if (surveying) {
                transform.Rotate(0, 60.0f, 0);
                DealWithSeenObjects();
            } else {
                surveying = true;
            }

            yield return new WaitForSeconds(rotationSpeed);
        }
    }
}
