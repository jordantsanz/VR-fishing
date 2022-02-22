using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBehavior : MonoBehaviour
{
    public GameObject fishManager;
    public int fishId;
    public int fishType;

    private float x;
    private float y;
    private float z;

    public float xVelocity;
    public float yVelocity;
    public float zVelocity; 

    public int frameCount;
    private int frameCountReset = 80;

    private Vector3 endPos;
    private Vector3 startPos;

    private int xBound;
    private float yBoundLower;
    private float yBoundUpper;
    private int zBound;

    private bool fishPaused;
    private int fishPausedFrameCount;
    private int fishPausedFrameCountReset;

    public bool caught;
    public bool inching;


    // Start is called before the first frame update
    void Start()
    {
        // get initial positions
        x = gameObject.transform.position.x;
        y = gameObject.transform.position.y;
        z = gameObject.transform.position.z;

        // get initial velocities
        xVelocity = Random.Range(-5, 5);
        yVelocity = Random.Range(-.2f, .2f);
        zVelocity = Random.Range(-5, 5);

        // get fish manager script and put id on script
        FishManagerScript sc = fishManager.GetComponent<FishManagerScript>();
        fishId = sc.fishSpawnId;

        // get boundaries
        xBound = sc.widthOfSpawnArea / 2;
        zBound = sc.heightOfSpawnArea / 2;
        yBoundLower = sc.minDepth;
        yBoundUpper = sc.depthOfSpawnArea;
        print(yBoundUpper + " y bound upper");

        // get start and end positions
        startPos = new Vector3(x, y, z);
        endPos = CheckBoundariesOfEndPos(new Vector3(x += xVelocity, y += yVelocity, z += zVelocity));

        frameCount = 0;
        fishPausedFrameCount = 0;
        fishPaused = false;
        caught = false;
        inching = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!caught && !inching)
        {
            if (!fishPaused)
            {
  
                MoveFish();
                frameCount += 1;
            }

            if (frameCount % frameCountReset == 0)
            {
                PauseFish();
            }
        }
        else
        {
            print("stopped moving: " + frameCount);
        }
    }


    void MoveFish()
    {
        float interpolationRatio = (float) frameCount / frameCountReset;
        Vector3 interpolatedPosition = Vector3.Lerp(startPos, endPos, interpolationRatio);
        gameObject.transform.position = interpolatedPosition;
        RotateToLookAt();

    }

    void ChangeDirection()
    {
        xVelocity = Random.Range(-10, 10);
        zVelocity = Random.Range(-10, 10);
        startPos = endPos;
        x = endPos.x;
        y = endPos.y;
        z = endPos.z;
        endPos = CheckBoundariesOfEndPos(new Vector3(x += xVelocity, y += yVelocity, z += zVelocity));
        fishPaused = false;
        frameCount = 0;

    }

    // adapted from https://answers.unity.com/questions/254130/how-do-i-rotate-an-object-towards-a-vector3-point.html
    void RotateToLookAt()
    {
        //find the vector pointing from our position to the target
        Vector3 targetPoint = endPos - transform.position;

        //create the rotation we need to be in to look at the target
        Quaternion _lookRotation = Quaternion.LookRotation(-targetPoint, Vector3.forward);

        //rotate us over time according to speed until we are in the required rotation 
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 2.0f);
    }


    Vector3 CheckBoundariesOfEndPos(Vector3 endPos1)
    {
 
        if (Mathf.Abs(endPos1.x) > xBound)
        {
            endPos1.x = SetPositionInBounds(endPos1.x, xBound);
        }

        if (Mathf.Abs(endPos1.z) > zBound)
        {
            endPos1.z = SetPositionInBounds(endPos1.z, zBound);
        }

        if (endPos1.y < yBoundUpper)
        {
            endPos1.y = yBoundUpper;
        }

        if (endPos1.y > yBoundLower)
        {
            endPos1.y = yBoundLower;
        }
        print(endPos1.y);
        return endPos1;
    }


     int SetPositionInBounds(float val, int bound)
    {
        if (val > 0)
        {
            return bound;
        }
        else
        {
            return -bound;
        }
    }

    void PauseFish()
    {
        if (!fishPaused)
        {
            fishPaused = true;
            fishPausedFrameCountReset = Random.Range(5, 200);
        }

        fishPausedFrameCount += 1;
        if (fishPausedFrameCount == fishPausedFrameCountReset)
        {
            frameCount = 0;
            fishPausedFrameCount = 0;
            ChangeDirection();
           
        }

   
    }
}
