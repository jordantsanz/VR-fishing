using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalableObject : MonoBehaviour
{
    public ControllerGrabber leftHand;
    public ControllerGrabber rightHand;
    private bool leftGrabbing;
    private bool rightGrabbing;

    private float initialDistance;
    private bool isResizing;
    private GameObject centerPivot;
    private ControllerGrabber primaryHand;


    // Start is called before the first frame update
    void Start()
    {
        leftGrabbing = false;
        rightGrabbing = false;
        isResizing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (leftHand.userGrab && GameObject.ReferenceEquals(this.gameObject, leftHand.grabbedObject))
        {
            leftGrabbing = true;
        }
        else
        {
            leftGrabbing = false;
        }

        if (rightHand.userGrab && GameObject.ReferenceEquals(this.gameObject, rightHand.grabbedObject))
        {
            rightGrabbing = true;
        }
        else
        {
            rightGrabbing = false;
        }

        InitResize(rightHand);

        if (isResizing)
        {
            SetScale();

        }
        
    }

    public void InitResize(ControllerGrabber mainController)
    {
        if (leftGrabbing && rightGrabbing)
        {
            isResizing = true;

            initialDistance = Vector3.Distance(mainController.transform.position, mainController.otherController.transform.position);
            centerPivot = new GameObject();
            Transform midpoint = centerPivot.transform;
            midpoint.position = (mainController.otherController.transform.position + mainController.transform.position) / 2;
            midpoint.rotation = CalculateRotation(mainController.transform, mainController.otherController.transform);

            primaryHand = mainController;

            transform.SetParent(midpoint);

            midpoint.SetParent(null);
        }
    }

    public Quaternion CalculateRotation(Transform t1, Transform t2)
    {
        Vector3 pos1 = t1.position;
        Vector3 pos2 = t2.position;

        Vector3 axis1to2 = pos2 - pos1;

        Vector3 y1 = t1.up;
        Vector3 y2 = t2.up;

        Vector3 averageY = (y1 + y2) / 2;

        Vector3 forward = Vector3.Cross(averageY, axis1to2);
        Vector3 finalY = Vector3.Cross(forward, axis1to2);

        Quaternion finalRotation = Quaternion.LookRotation(forward, finalY);

        return finalRotation;

    }

    void SetScale()
    {
        Vector3 primaryPosition = primaryHand.transform.position;
        Vector3 secondaryPosition = primaryHand.otherController.transform.position;

        float scale = Vector3.Distance(primaryPosition, secondaryPosition) / initialDistance;

        centerPivot.transform.localScale = new Vector3(scale, scale, scale);
        centerPivot.transform.position = (primaryPosition + secondaryPosition) / 2;
        centerPivot.transform.rotation = CalculateRotation(primaryHand.transform, primaryHand.otherController.transform);
        isResizing = false;
    }
}
