using UnityEngine;

public class ForceGun : MonoBehaviour {

    public Transform gunTip, camera, player;
    public LayerMask whatIsGrappleable;
    private float maxDistance = 100f;

    public float slapForce = 400f;

    void Awake() {

    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            StartSlappin();
        }
        else if (Input.GetMouseButtonUp(0)) {
            StopSlappin();
        }
    }

    //Called after Update
    void LateUpdate() {

    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartSlappin() {
        Debug.Log("start slappin");
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable)) {
            Vector3 incomingVec = hit.point - gunTip.position;
            hit.rigidbody.AddForce(incomingVec.normalized * slapForce);
            Debug.Log("slap");
        }
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopSlappin() {

    }
}