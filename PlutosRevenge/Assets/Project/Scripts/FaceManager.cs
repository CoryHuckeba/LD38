using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceManager : Singleton<FaceManager> {

    #region Cached Components

    // Sprite Objects
    public GameObject SPRITE_IDLE;
    public GameObject SPRITE_GRAV;
    public GameObject SPRITE_FAST;
    public GameObject SPRITE_HURT;
    
    #endregion Cached Components


    #region Properties & Variables

    // Dict for easy lookup
    private Dictionary<PlutoFaces, GameObject> faces = new Dictionary<PlutoFaces, GameObject>();

    // Tracking Variables
    private float z = -.14f;
    private Transform player;

    #endregion Properties & Variables


    #region MonoBehaviour Implementation

    // Use this for initialization
    void Start ()
    {
        // Assemble Face Dictionary
        faces.Add(PlutoFaces.Default, SPRITE_IDLE);
        faces.Add(PlutoFaces.MovingFast, SPRITE_FAST);
        faces.Add(PlutoFaces.Gravity, SPRITE_GRAV);
        faces.Add(PlutoFaces.Hurt, SPRITE_HURT);

        // Store reference to player
        player = PlutoController.Instance.gameObject.transform;

        SetFace(PlutoFaces.Default);
	}

    void Update()
    {
        Vector3 newTransform = player.position;
        transform.position = new Vector3(newTransform.x, newTransform.y, z);
    }

    #endregion MonoBehaviour Implementation


    #region Public Interface

    public void SetFace(PlutoFaces face)
    {
        GameObject newFaceObj = faces[face];

        foreach(KeyValuePair <PlutoFaces, GameObject> kvp in faces)
        {
            if (kvp.Value != newFaceObj)
                kvp.Value.SetActive(false);
            else
                kvp.Value.SetActive(true);            
        }
    }

    #endregion Public Interface
}
