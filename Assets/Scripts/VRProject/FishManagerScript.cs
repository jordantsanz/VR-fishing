using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManagerScript : MonoBehaviour
{
    public List<GameObject> fishToSpawn;
    public List<GameObject> fishInPlay;
    public int fishSpawnId = 0;
    public int rateToSpawnFish;
    public int fishInPlayCap;
    public int currentFishInPlay;
    public Transform center;
    public int widthOfSpawnArea;
    public int heightOfSpawnArea;
    public float depthOfSpawnArea;
    public float minDepth;
    public int randomFishInt;



    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnFish", rateToSpawnFish, rateToSpawnFish);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnFish()
    {
           
            randomFishInt = Random.Range(0, fishToSpawn.Count - 1);
            GameObject randomFish = fishToSpawn[randomFishInt];
            fishSpawnId += 1;
            int randomX = Random.Range(-widthOfSpawnArea / 2, widthOfSpawnArea / 2);
            int randomZ = Random.Range(-heightOfSpawnArea / 2, heightOfSpawnArea / 2);
            float randomY = Random.Range(-Mathf.Abs(minDepth), -Mathf.Abs(depthOfSpawnArea));
            GameObject obj = Instantiate(randomFish, new Vector3(randomX, randomY, randomZ), Quaternion.Euler(0, 0, -90));
            fishInPlay.Add(obj);
            currentFishInPlay += 1;
            
    }
}
