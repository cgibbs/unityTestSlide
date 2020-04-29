using UnityEngine;

public class GrapplingGunTether : MonoBehaviour {

    private LineRenderer lr;
    public Vector3 grapplePoint;
    public LayerMask whatIsGrappleable;
    public Transform gunTip, camera, player;
    private float maxDistance = 100f;
    public SpringJoint joint;
    private bool inTether = false;

    public Collider grappleBody;

    void Awake() {
        lr = GetComponent<LineRenderer>();
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            StartTether();
        }
        else if (Input.GetMouseButtonUp(0)) {
            StopTether();
        }
    }

    //Called after Update
    void LateUpdate() {
        DrawRope();
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    void StartTether() {
        Debug.Log("tether start");
        RaycastHit hit;
        if (!inTether && Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable)) {
            grapplePoint = hit.point;
            grappleBody = hit.collider;
            inTether = true;
            Debug.Log("now in tether");
        }
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopTether() {
        Debug.Log("stop tether");
        RaycastHit hit;
        if (Physics.Raycast(camera.position, camera.forward, out hit, maxDistance, whatIsGrappleable)) {
            Debug.Log("found tether object");
            currentGrapplePosition = hit.point;
            // TODO: this needs to be attached to the object that's hit, instead
            //joint = player.gameObject.AddComponent<SpringJoint>();
            joint = grappleBody.gameObject.AddComponent<SpringJoint>();
            //joint.anchor = grapplePoint;
            joint.autoConfigureConnectedAnchor = true;
            joint.connectedBody = hit.rigidbody;
            //joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            //The distance grapple will try to keep from grapple point. 
            joint.maxDistance = distanceFromPoint * 0.8f;
            //joint.minDistance = distanceFromPoint * 0.25f;
            joint.minDistance = 0;

            //Adjust these values to fit your game.
            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 5.0f;

            lr.positionCount = 2;
            currentGrapplePosition = hit.point;
            inTether = false;
        } else {
            inTether = false;
            lr.positionCount = 0;
            Debug.Log("destroying joint");
            Destroy(joint);
        }
    }

    public Vector3 currentGrapplePosition;
    
    void DrawRope() {
        //If not grappling, don't draw rope
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);
        
        lr.SetPosition(0,  grapplePoint);
        lr.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling() {
        return joint != null;
    }

    public Vector3 GetGrapplePoint() {
        return grapplePoint;
    }
}