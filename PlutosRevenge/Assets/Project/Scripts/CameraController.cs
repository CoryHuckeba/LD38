using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    #region Cached Components

    public Camera myCamera;
    public Transform target;

    #endregion Cached Components


    #region Properties & Variables

    // Follow params
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;

    // Zoom params
    public float minimumZoom = 10f;
    public float maximumZoom = 25f;

    #endregion Properties & Variables


    #region MonoBehaviourImplementation

    // Use this for initialization
    void Start ()
    {

    }

    void FixedUpdate()
    {
        // Follow
        if (target)
        {
            Vector3 point = myCamera.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - myCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); 
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }

        // TODO: Tween this for maximum smooooooooothness
        // Set the zoom level
        myCamera.orthographicSize = minimumZoom + (maximumZoom - minimumZoom) * PlutoController.Instance.speedRatio;
    }

    #endregion MonoBehaviourImplementation
}
