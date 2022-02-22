using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class FishingScript : MonoBehaviour
{
    private GameObject fish;
    public GameObject fishManager;
    public List<int> fishCaught = new List<int>();
    public List<GameObject> fishCaughtToSpawn = new List<GameObject>();
    public bool catchingFish = false;
    public List<GameObject> caughtFishSpawnpoints;
    public AudioSource fishOnDockSound;
    public int totalFishCaught;

    // Start is called before the first frame update
    void Start()
    {
        fish = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (catchingFish)
        {
            CatchFishSequence();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Fish" && fish == null)
        {
            fish = other.gameObject;
            catchingFish = true;

            OVRInput.SetControllerVibration(0.1f, 0.1f, OVRInput.Controller.RTouch);
            OVRInput.SetControllerVibration(0.1f, 0.1f, OVRInput.Controller.LTouch);
            fish.transform.parent = gameObject.transform;
            fish.gameObject.transform.rotation = Quaternion.Euler(-1, -32, 141);
            GetComponentInChildren<MoveCloserScript>().lureStatus = 2;
            BoxCollider[] colliders = fish.GetComponentsInChildren<BoxCollider>();
            for (int i = 0; i < colliders.Length; i += 1)
            {
                Destroy(colliders[i]);
            }

            Destroy(fish.GetComponent<BoxCollider>());
            Destroy(fish.GetComponent<CapsuleCollider>());
   

            // caught fish
            fish.gameObject.GetComponent<FishBehavior>().caught = true;
            fishManager.GetComponent<FishManagerScript>().fishInPlay.Remove(fish);

        }


        else if (other.tag == "Dock" && fish != null)
        {
            print("Collided with dock");
            catchingFish = false;

            // get right kind of fish
            int index = fish.gameObject.GetComponent<FishBehavior>().fishType;
            print("index: " + index);

            if (!fishCaught.Contains(index))
            {
                
                GameObject newFish = Instantiate(fishCaughtToSpawn[index], caughtFishSpawnpoints[index].transform);
                fishCaught.Add(index);
            }

            
            Destroy(fish.gameObject);
            fish = null;
            print("Destroyed old fish");
            fishManager.GetComponent<FishManagerScript>().currentFishInPlay -= 1;
            fishOnDockSound.Play();
            totalFishCaught += 1;

        }
    }

    void CatchFishSequence()
    {
   
        fish.transform.parent = gameObject.transform;
        fish.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - 1, gameObject.transform.position.z);

    }
}
