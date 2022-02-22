using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickGrab : MonoBehaviour
{
    public GameObject handleObj;
    public Transform resetPoint;
    private Transform handle;
    private bool _isGrabbed;

    // Start is called before the first frame update
    void Start()
    {
        handle = handleObj.transform;
        
    }

    // Update is called once per frame
    void Update()
    {

        // Resets handle position back to initial position when not being grabbed
        _isGrabbed = handleObj.GetComponent<OVRGrabbable>().isGrabbed;
        if (!_isGrabbed)
        {
            handle.position = new Vector3(resetPoint.position.x,resetPoint.position.y,resetPoint.position.z);
        }


        // rotate joystick towards handle
        Vector3 targetView = new Vector3(transform.position.x, handle.position.y, handle.position.z);
        transform.LookAt(targetView, transform.up);

        // clamps joystick to not go beyond 30 degrees
        if (transform.localRotation.eulerAngles.x >= 0 && transform.localRotation.eulerAngles.x <= 180)
        {
            transform.localEulerAngles = new Vector3(Mathf.Clamp(transform.localRotation.eulerAngles.x,0,30), transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
        }
        else
        {
            // use range of [360,330] instead of [0,-30] because euler doesn't use negatives
            transform.localEulerAngles = new Vector3(Mathf.Clamp(transform.localRotation.eulerAngles.x,330,360), transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
        }


        
    }
}
