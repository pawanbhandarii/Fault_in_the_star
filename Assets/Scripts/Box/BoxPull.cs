using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPull : MonoBehaviour
{
    bool beingPushed;
    float x_Position;

    // Start is called before the first frame update
    void Start()
    {
        x_Position = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (beingPushed == false)
        {
            transform.position = new Vector3(x_Position, transform.position.y, transform.position.z);
        }
        else
        {
            x_Position = transform.position.x;
        }
    }

    public void SetState(bool pushed)
    {
        beingPushed = pushed;
    }
}
