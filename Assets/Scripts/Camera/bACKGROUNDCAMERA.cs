using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bACKGROUNDCAMERA : MonoBehaviour
{
    public GameObject cam;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(cam.transform.position.x, transform.position.y, transform.position.z);
    }
}
