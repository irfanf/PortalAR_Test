using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

#if UNITY_EDITOR
using input = GoogleARCore.InstantPreviewInput;
#endif
public class ARController : MonoBehaviour
{

    //planes that ARCore detected in the current frames
    private List<TrackedPlane> newTrackedPlanes = new List<TrackedPlane>();

    public GameObject GridPrefab;

    public GameObject Portal;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Check ARCore session status
        if (Session.Status != SessionStatus.Tracking) return;

        //fill newTrackedPlanes with new planes
        Session.GetTrackables<TrackedPlane>(newTrackedPlanes, TrackableQueryFilter.New);

        for (int i = 0; i < newTrackedPlanes.Count; i++)
        {
            GameObject grid = Instantiate(GridPrefab, Vector3.zero, Quaternion.identity, transform);

            //this func will set the position of grid and modify the vertices of the attached mesh
            grid.GetComponent<GridVisualizer>().Initialize(newTrackedPlanes[i]);
        }

        //Check if the used touches the screen
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began) return;


        //Let's now check if the user touched any of the tracked planes
        TrackableHit hit;
        if( Frame.Raycast(touch.position.x, touch.position.y, TrackableHitFlags.PlaneWithinPolygon, out hit))
        {
            // Let's now place the portal on top of the tracked plane that we touched

            //Enable the portal
            Portal.SetActive(true);

            //Create a new Anchor
            Anchor anchor = hit.Trackable.CreateAnchor(hit.Pose);

            //Set the posiution of the portal to be the same as the hit position
            Portal.transform.position = hit.Pose.position;
            Portal.transform.rotation = hit.Pose.rotation;

            //We want the portal to face the camera
            Vector3 cameraPosition = Camera.main.transform.position;

            //Rotate the portal to face the camera
            Portal.transform.LookAt(cameraPosition, Portal.transform.up);

            //ARCore will keep understanding the woreld and update the anchors accordingly hence we need to attach our portal top the anchor
            Portal.transform.parent = anchor.transform;
        }
    }
}
