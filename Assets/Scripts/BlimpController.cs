using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class BlimpController : MonoBehaviour
{
    [Header("------Main Control------")]

    public float damper;
    public float maxSpeed;
    private float leverThreshold;
    public SteeringWheelManager steeringWheel;
    public GameObject propeller;
    private Rigidbody rigidbody;


    [Header("------Thrust------")]
    public GameObject thrustLever;
    public float currentThrust;
    public float targetThrust;
    private float thrustDiff;
    private HingeJoint thrustHinge;
    private float thrustAngle;

    [Header("------Altitude------")]
    public GameObject altLever;
    public float currentRiseSpeed;
    public float targetRiseSpeed;
    private float altDiff;
    private HingeJoint altHinge;
    private float altAngle;


    [Header("------Rudder & Turning------")]
    public GameObject rudder;
    public float rudderTargetAngle;
    private float rudderMaxAngle = 45;
    public float rudderCurrentAngle;
    public float maxAngularVelocity;
    private float turnDiff;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        // grab the angle of the altitude lever
        altHinge = altLever.GetComponent<HingeJoint>();
        altAngle = altHinge.angle;

        // grab the angle of the thrust lever
        thrustHinge = thrustLever.GetComponent<HingeJoint>();
        thrustAngle = thrustHinge.angle;

        // calculate the thrust change
        targetThrust = thrustAngle / -6;
   
        thrustDiff = targetThrust - currentThrust;
        if(Mathf.Abs(thrustDiff) > leverThreshold)
        {
            currentThrust += thrustDiff / (damper * 60);
        }

        // calculate the altitude change
        targetRiseSpeed = altAngle / -6;
       
        altDiff = targetRiseSpeed - currentRiseSpeed;
        if (Mathf.Abs(altDiff) > leverThreshold)
        {
            currentRiseSpeed += altDiff / (damper * 60);
        }

        // calculate the wheel angle
        rudderTargetAngle = steeringWheel.x / 4;

        turnDiff = rudderTargetAngle - rudderCurrentAngle;
        if (Mathf.Abs(rudderCurrentAngle) < rudderMaxAngle)
        {
            rudderCurrentAngle += turnDiff / (damper * 60);
        }
        rudder.transform.localEulerAngles = new Vector3(0, rudderCurrentAngle, 0);
    }

    //better for physics
    private void FixedUpdate()
    {
        // if not at max speed, move the blimp according to the thrust/altitude
        if (rigidbody.velocity.magnitude < maxSpeed / 10)
        {
            rigidbody.AddForce(this.transform.forward * currentThrust * -1);
            rigidbody.AddForce(this.transform.up * currentRiseSpeed);
        }

        //if not at max angular velocity, turn the blimp according to the wheel
        if (rigidbody.angularVelocity.magnitude < maxAngularVelocity / 500)
        {
            rigidbody.AddTorque(this.transform.up * rudderCurrentAngle * currentThrust / 1000 * -1);
        }
        propeller.GetComponent<Rigidbody>().AddTorque(propeller.transform.forward * currentThrust * -12);
        
    }

  
}
