using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class BeaconPlacement : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public GameObject beaconPrefab;

    private static List<ARRaycastHit> _hits = new List<ARRaycastHit>();

    void Update()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase != TouchPhase.Began)
            return;

        Vector2 touchPosition = touch.position;

        if (raycastManager.Raycast(touchPosition, _hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = _hits[0].pose;
            Instantiate(beaconPrefab, hitPose.position, hitPose.rotation);
        }
    }
}