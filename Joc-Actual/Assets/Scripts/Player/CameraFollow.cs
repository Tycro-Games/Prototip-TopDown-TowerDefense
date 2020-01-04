using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = .3f;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;
    private void Update()
    {
        //postion desired
        Vector3 desiredPos = target.position + offset;
        Vector3 SmoothedPos = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, smoothSpeed);
        transform.Translate(desiredPos);
    }

}
