using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyclistSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject cyclist1Prefab; //Single 1
    public GameObject cyclist2Prefab; //Single 2
    public GameObject cyclist3Prefab; //Single 3
    public GameObject cyclist4Prefab; //Single 4
    public GameObject cyclist5Prefab; //Single 5
    public GameObject cyclist6Prefab; //Single 6
    public GameObject cyclist1Instance;
    public GameObject cyclist2Instance;
    public GameObject cyclist3Instance;
    public GameObject cyclist4Instance;
    public GameObject cyclist5Instance;
    public GameObject cyclist6Instance;

    [Header("Variables")]
    public float minWait;
    public float maxWait;
    public float minSpeed;
    public float maxSpeed;
    public int maxPrefabNumber; //Set this 1 higher than the amount of options
    public bool goLeft;

    [Header("Internal maths")]
    public float chosenWait;
    public float currentWait;
    public float chosenSpeed;
    public int chosenPrefabNumber;
    // Start is called before the first frame update
    void Start()
    {
        chosenWait = Random.Range(minWait, maxWait);
        chosenPrefabNumber = Random.Range(1, maxPrefabNumber);
        chosenSpeed = Random.Range(minSpeed, maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWait < chosenWait)
        {
            currentWait += Time.deltaTime;
        }
        else
        {
            currentWait = 0;
            SpawnCyclists();
            chosenWait = Random.Range(minWait, maxWait);
        }
    }
    public void SpawnCyclists()
    {
        if (chosenPrefabNumber == 1)
        {
            cyclist1Instance = Instantiate(cyclist1Prefab, transform.position, Quaternion.identity);
            Cyclist cyclist = cyclist1Instance.GetComponent<Cyclist>();
            if (goLeft)
            {
                cyclist.speed = -chosenSpeed;
            }
            else
            {
                cyclist.speed = chosenSpeed;
            }
        }
        else if (chosenPrefabNumber == 2)
        {
            cyclist2Instance = Instantiate(cyclist2Prefab, transform.position, Quaternion.identity);
            Cyclist cyclist = cyclist2Instance.GetComponent<Cyclist>();
            if (goLeft)
            {
                cyclist.speed = -chosenSpeed;
            }
            else
            {
                cyclist.speed = chosenSpeed;
            }
        }
        else if (chosenPrefabNumber == 3)
        {
            cyclist3Instance = Instantiate(cyclist3Prefab, transform.position, Quaternion.identity);
            Cyclist cyclist = cyclist3Instance.GetComponent<Cyclist>();
            if (goLeft)
            {
                cyclist.speed = -chosenSpeed;
            }
            else
            {
                cyclist.speed = chosenSpeed;
            }
        }
        else if (chosenPrefabNumber == 4)
        {
            cyclist4Instance = Instantiate(cyclist4Prefab, transform.position, Quaternion.identity);
            Cyclist cyclist = cyclist4Instance.GetComponent<Cyclist>();
            if (goLeft)
            {
                cyclist.speed = -chosenSpeed;
            }
            else
            {
                cyclist.speed = chosenSpeed;
            }
        }
        else if (chosenPrefabNumber == 5)
        {
            cyclist5Instance = Instantiate(cyclist5Prefab, transform.position, Quaternion.identity);
            Cyclist cyclist = cyclist5Instance.GetComponent<Cyclist>();
            if (goLeft)
            {
                cyclist.speed = -chosenSpeed;
            }
            else
            {
                cyclist.speed = chosenSpeed;
            }
        }
        else if (chosenPrefabNumber ==63)
        {
            cyclist6Instance = Instantiate(cyclist6Prefab, transform.position, Quaternion.identity);
            Cyclist cyclist = cyclist6Instance.GetComponent<Cyclist>();
            if (goLeft)
            {
                cyclist.speed = -chosenSpeed;
            }
            else
            {
                cyclist.speed = chosenSpeed;
            }
        }
        chosenPrefabNumber = Random.Range(1, maxPrefabNumber);
    }
}



//Wait random amount of seconds
//Decide how much to spawn
//Spawn some cyclists with random images and speeds
//Wait again