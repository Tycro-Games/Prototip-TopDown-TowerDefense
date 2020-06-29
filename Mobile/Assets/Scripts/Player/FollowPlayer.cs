using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
    public Rigidbody target;            // The position that that camera will be following.
    public float smoothing = 5f;        // The speed with which the camera will be following.

    Vector3 offset;                     // The initial offset from the target.

    void Start()
    {
        // Calculate the initial offset.
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        offset = transform.position - target.position;
    }

    void Update()
    {
        // Create a postion the camera is aiming for based on the offset from the target
        if (target != null)
        {
            Vector3 targetCamPos = target.position + offset;

            // Smoothly interpolate between the camera's current position and it's target position.
            transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
        }
    }
}