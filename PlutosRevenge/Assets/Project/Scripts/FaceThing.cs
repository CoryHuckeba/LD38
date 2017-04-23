using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceThing : MonoBehaviour
{
    public GameObject player;
    public float z = -.14f;

    void Update()
    {
        Vector3 newTransform = player.transform.position;
        transform.position = new Vector3(newTransform.x, newTransform.y, z);
    }
}
