using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public GameObject platform; // refernce to the platformer to move

    public GameObject[] myWayPoints; // array of all the way points

    public GameObject Switch;

    [Range(0.0f, 10.0f)] // Creates a slider from 0 to 10 in inspector
    public float moveSpeed = 5f; // Move speed of the platform
    public float waitAtWaypointTime = 1f; //Wait time after reaching the platform

    public bool loop = true;

    // private variables

    Transform transform;
    int myWayPointIndex = 0; 
    float movingTime;
    bool moving;

    // Start is called before the first frame update
    void Start()
    {
        //Initialization
        transform = platform.transform;
        movingTime = 0f;
        moving = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if beyound move time then start moving
        if (Time.time >= movingTime)
        {
            Movement();
        }
    }

    void Movement()
    {
        if (Switch.GetComponent<LeverSwitch>().isOn)
        {
            //checking if there are way points
            if (myWayPoints.Length != 0 && moving)
            {
                //moves towards a way point
                transform.position = Vector3.MoveTowards(transform.position, myWayPoints[myWayPointIndex].transform.position, moveSpeed * Time.deltaTime);

            }

            //changing the waypoint of the platform
            if (Vector3.Distance(myWayPoints[myWayPointIndex].transform.position, transform.position) <= 0)
            {
                ++myWayPointIndex;
                movingTime = Time.time + waitAtWaypointTime;
            }

            //reset the waypoint back to 0 for looping otherwise stop the movement
            if (myWayPointIndex >= myWayPoints.Length)
            {
                if (loop)
                {
                    myWayPointIndex = 0;
                }
                else
                {
                    moving = false;
                }
            }
        }
        

    }
}
