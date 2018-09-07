using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class ARController : MonoBehaviour
{

    //planes that ARCore detected in the current frames
    private List<TrackedPlane> newTrackedPlanes = new List<TrackedPlane>();

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
    }
}
