using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCloserScript : MonoBehaviour
{
    public GameObject lureBite;
    public FishingScript fishingScript;
    public int lureStatus; // 0: not close to lure, 1: within trigger box of lure, 2: attached to lure
    private GameObject fish;
    public int framesNecessaryToMoveTowardsLure;
    private int framesMoving;
    private Vector3 startPos;
    private bool catchingFish;

    private int xBound;
    private float yMinDepth;
    private float yMaxDepth;
    private int zBound;

    public FishManagerScript sc;

    // Start is called before the first frame update
    void Start()
    {
        lureStatus = 0;
        framesMoving = 0;
        fish = transform.parent.gameObject;
        catchingFish = fishingScript.catchingFish;

        lureBite = GameObject.Find("LureBite");

        // get boundaries
        xBound = sc.widthOfSpawnArea / 2;
        zBound = sc.heightOfSpawnArea / 2;
        yMinDepth = sc.minDepth;
        yMaxDepth = sc.depthOfSpawnArea;

    }

    // Update is called once per frame
    void Update()
    {
        
            if (lureStatus == 1 && !catchingFish) 
            {
                framesMoving += 1;

                Vector3 interpolatedPosition = Vector3.Lerp(startPos, lureBite.transform.position, (float)framesMoving / framesNecessaryToMoveTowardsLure);
                fish.transform.position = interpolatedPosition;

                //find the vector pointing from our position to the target
                Vector3 targetPoint = lureBite.transform.position - transform.position;

                //create the rotation we need to be in to look at the target
                Quaternion _lookRotation = Quaternion.LookRotation(-targetPoint, Vector3.forward);

                //rotate us over time according to speed until we are in the required rotation 
                transform.rotation = _lookRotation;

                if (framesMoving == framesNecessaryToMoveTowardsLure)
                {
                    lureStatus = 2;
                }

            }

        catchingFish = fishingScript.catchingFish;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Lure" && lureBite.transform.position.y < yMinDepth) // negative val
        {
            
            lureStatus = 1;
            fish.GetComponent<FishBehavior>().inching = true;
            float x = fish.transform.position.x;
            float y = fish.transform.position.y;
            float z = fish.transform.position.z;
            startPos = new Vector3(x, y, z);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Lure")
        {
            lureStatus = 0;
            fish.GetComponent<FishBehavior>().inching = false;
        }
    }
}
