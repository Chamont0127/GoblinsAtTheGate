using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    #region [Variables]
    [SerializeField] private Vector3 startingPos;
    [SerializeField] private GameObject goblinPrefab;
    [SerializeField] private GameObject colossusPrefab;
    [SerializeField] private float countdown;
    [SerializeField] private int goblinsPerWave = 2;
    private GameManager GameManager;
    #endregion

    #region [Variable Properties]
    public float Countdown
    {
        get => countdown;
        
        set
        {
            if(value >= 0)
            {
                countdown = value;
            }
            else
                countdown = 0;
        }
    }
    #endregion

    // sets starting Position of enemies to the spawnpoint position
    void Start()
    {
        startingPos = transform.position; 
        GameManager = GameObject.Find("Main Camera").GetComponent<GameManager>();
    }

    //counts down to the next wave of enemies and calls SpawnWave
    void Update()
    {
        if(Countdown <= 0 && GameManager.GameIsActive)
        {
            StartCoroutine(SpawnWave());
            Countdown = 5;
        }

        Countdown -= Time.deltaTime;
    }

    //spawns the next wave of enemies by calling SpawnGoblin and Spawn Colossus a set number of times
    IEnumerator SpawnWave()
    {
        for(int i = 0; i < goblinsPerWave; i++)
        {
            SpawnGoblin();
            yield return new WaitForSeconds (0.5f);
        }

        SpawnColossus();
        yield return new WaitForSeconds (0.5f);
    }

    //Spawns a goblin
    void SpawnGoblin()
    {
        Instantiate(goblinPrefab, startingPos, transform.rotation);
    }

    //Spawns a colossus
    void SpawnColossus()
    {
        Instantiate(colossusPrefab, startingPos, transform.rotation);
    }


//    WaveController - https://www.w3schools.com/cs/cs_switch.php
//        int waveindex = 1
//        int[] goblinsToSpawn = [5,8,10,15,5,20,30,40,50,100]
//        int[] colossusToSpawn = [1,5]
//        switch(Waveindex)
//            default case
//                for(int i = 0; i < goblinsToSpawn[waveIndex];i++)
//                  {SpawnGoblin();
//                  }
//                  break
//            case: 5
//                spawn boss wave 5
//                  break
//            case: 10 (spawn boss wave 10)
// Can also use this system to start and stop the different music in the audio controller 
// Need to figure out how to make sure only one wave is playing at a time? - https://forum.unity.com/threads/how-to-make-infinite-wave-spawner-with-different-enemy-types.1067810/
}
