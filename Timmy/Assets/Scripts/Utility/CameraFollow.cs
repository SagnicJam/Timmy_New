using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothSpeed=10f;
    public Transform target;

    public bool doLerp;
    Vector3 offset;

    private void Start()
    {
        offset = transform.position - target.position;
    }

    private void FixedUpdate()
    {
        Vector3 newPosition = new Vector3(transform.position.x,transform.position.y,offset.z+target.position.z);
        if(doLerp)
        {
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.fixedDeltaTime * smoothSpeed);
        }
        else
        {
            transform.position = newPosition;
        }
    }
}
