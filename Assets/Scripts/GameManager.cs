using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public List<GameObject> slices;
    public GameObject currentToaster;
    public GameObject titleObject;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI gameOverText;
    private int totalScore = 0;    
    public bool isGameActive;
    [SerializeField] Material toastedMaterial;

    //ENCAPSULATION
    public Material ToastedMaterial { get { return toastedMaterial; } }

    private ToasterMgr toasterScript;  //this is a clever shortcut to the script attached to the toaster object.
    //ENCAPSULATION
    public ToasterMgr ToasterScript { get { return toasterScript; } }  //this is the public handle to the toaster script.

    private GameObject startBtn;
    private Vector3 starter_position;
    private float spawnRate = 1.0f;
    private int potentialPlate;
    private int timeLeft;
    private bool[] plate_in_use = new bool[] { false, false, false, false };  //this does 4 plates.  probably could be re-done for N plates and pass them in.
    private readonly Vector3[] plate_positions = {new Vector3(-2.6f, 0.4f, -0.29f),  //this hard-codes the locations of each plate.  Probably needs to be re-created to subtract 2.6 from each one.
                                                  new Vector3(-5.2f, 0.4f, -0.29f),
                                                  new Vector3(-7.8f, 0.4f, -0.29f),
                                                  new Vector3(-10.4f,0.4f, -0.29f)
                                                 };

    // Start is called before the first frame update
    void Start()
    {
        toasterScript = currentToaster.GetComponent<ToasterMgr>();
        startBtn = GameObject.FindGameObjectWithTag("starter");
    }

    IEnumerator SpawnSlice()
    {
        //this function creates slices of bread at pre-determined positions.
        //uses private variables to set locations.


        // stop trying to create new slices once game is over.
        while (isGameActive)
        {
            yield return new WaitForSeconds(spawnRate);
            if (plate_in_use.Contains(false))  //only execute the following code if there is a spot to hold a slice
            {
                do 
                {
                    potentialPlate = UnityEngine.Random.Range(0, 4); //start with a random number between 0 and 3... this is a potential spot for our slice
                }
                while (plate_in_use[potentialPlate]);  //check if that spot is available.  "false" = "available"... so if potentialPlate = 2 and spot 2 is free, this will return FALSE,
                                                       // and break the loop.  if potentialPlate is 2 and spot 2 is occupied, this will return TRUE and the loop repeats.

                plate_in_use[potentialPlate] = true;   //if we got this far, then this spot is free.  Therefore mark it as "occupied"
                starter_position = plate_positions[potentialPlate];  //now get the actual location to use from the positions array (a set of vector3's)
                int idx = UnityEngine.Random.Range(0, slices.Count); //then generate a random number from 0 to however many slices gamemanager has defined...
                var slice = Instantiate(slices[idx], starter_position, slices[idx].transform.rotation);  //and instantiate that slice, at the selected starting position, and at the prefab's rotation settings.
                slice.GetComponent<BreadScript>().PlateNumber = potentialPlate;
            }
            else
            {
                //Debug.Log("No Plate Available");
            }

        }
    }
    public void StartGame(int difficulty)
    {
        //there's going to be all sorts of problems if the passed difficulty is negative (or zero) so just confirm. 
        if (difficulty > 0)
        {
           
            //clean up the environment, removing the start button and text.  Also destroy any previous bread slices
            startBtn.SetActive(false);
            titleObject.SetActive(false);
            gameOverText.gameObject.SetActive(false);
            plate_in_use = new bool[] { false, false, false, false };
            DestroyPrefabsInScene("bread");
            
            scoreText.text = "Score: " + totalScore;
            timeLeft = 10;
            spawnRate /= difficulty; //divide default spawnRate by the passed difficulty.  difficulty 2 would be 2x as fast, 3 would be 3x as fast.
            isGameActive = true;

            totalScore = 0;
            ScorePoints(0);

            StartCoroutine(SpawnSlice());
            StartCoroutine(CountdownTimer());
        } else { 
            Debug.Log("difficulty needs to be an int greater than zero!");
        }
    }

    //ABSTRACTION
    void DestroyPrefabsInScene(string tagName)
    {
        GameObject[] prefabs = GameObject.FindGameObjectsWithTag(tagName);
        foreach (GameObject prefab in prefabs)
        {
            Destroy(prefab);
        }
    }
    
    public void ReleasePlate(int plate)
    {
        if (plate < plate_in_use.Length)
        {
            plate_in_use[plate] = false;
        } else
        {
            Debug.Log("Bad Plate Location: " + plate);
            Debug.Log("slices.Count == " + plate_in_use.Length);
        }
    }

    //ABSTRACTION
    public void ScorePoints(int newPoints)
    {
        if (newPoints >= 0)
        {
            totalScore += newPoints;
            scoreText.text = "Score: " + totalScore;
            timeLeft += newPoints + 1; //not sure what the right number should be. 2?
        }
    }
    IEnumerator CountdownTimer()
    {
        while (isGameActive)
        {
            yield return new WaitForSecondsRealtime(1);
            if (timeLeft == 0) GameOver();
            if (isGameActive)
            {
                UpdateTime(1);
            }
        } 
    }
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        titleObject.SetActive(true);
        startBtn.SetActive(true);
        isGameActive = false;
    }
    public void UpdateTime(int timeToRemove)
    {
        timeLeft -= timeToRemove;
        timerText.text = "Time: " + timeLeft;
    }
}
