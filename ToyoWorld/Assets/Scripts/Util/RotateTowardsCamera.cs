using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsCamera : MonoBehaviour
{
    Transform cam;
    private void Start()
    {
        cam = Camera.main?.transform;
    }

    void Update()
    {
        if (cam == null) return;

        // Make the object look at the camera
        transform.forward = cam.forward;
    }
}
