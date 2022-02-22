using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringWheelManager : MonoBehaviour
{
    public OVRGrabbable[] targets;
    private Dictionary<OVRGrabbable, Vector3> targetLocalPos;
    private Dictionary<OVRGrabbable, Quaternion> targetLocalRot;
    public Transform grabPointFolder;
    public Vector3 up;
    public HingeJoint hinge;
    public float x;
    public float cacheAngle;
    private bool onGrab;
    private bool nextFrame;
    public float jumpAngle;
    private bool anyGrabbed;
    public float hingeAngle;
    // Start is called before the first frame update
    void Start()
    {
        hinge = this.GetComponentInChildren<HingeJoint>();
        targetLocalPos = new Dictionary<OVRGrabbable, Vector3>();
        targetLocalRot = new Dictionary<OVRGrabbable, Quaternion>();
        foreach (OVRGrabbable target in targets)
        {
            targetLocalPos.Add(target, target.transform.localPosition);
            targetLocalRot.Add(target, target.transform.localRotation);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        hingeAngle = hinge.angle;
        anyGrabbed = false;
        if (nextFrame)
        {
            jumpAngle = cacheAngle - hinge.angle;
            nextFrame = false;
        }

        foreach (OVRGrabbable target in targets)
        {
            if (target.isGrabbed)
            {
                anyGrabbed = true;
                Vector3 targetInLine = target.transform.position;
                targetInLine.z = transform.position.z;
                transform.LookAt(targetInLine, up);
                transform.Rotate(0, 0, 90);
                

                if (onGrab)
                { //do something once for each grab
                    nextFrame = true;
                    onGrab = false;
                }
            }
            else
            {
                target.transform.parent = grabPointFolder;
                target.transform.localPosition = targetLocalPos[target];
                target.transform.localRotation = targetLocalRot[target];
                target.GetComponent<Rigidbody>().velocity = Vector3.zero;
                target.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }

        if (!anyGrabbed)
        {
            onGrab = true;
            cacheAngle = hinge.angle;

        }

        x = hinge.angle + jumpAngle;

        

        grabPointFolder.localEulerAngles = new Vector3(hinge.angle, 90, 0);
        //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, -90, 0);
    }
}
