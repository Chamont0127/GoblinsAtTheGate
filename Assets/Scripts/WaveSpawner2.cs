using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner2 : MonoBehaviour
{
 #region [Variables]
    [SerializeField] private Vector3 startingPos;
    [SerializeField] private GameObject goblinPrefab;
    [SerializeField] private GameObject colossusPrefab;
    [SerializeField] private float countdown;
    [SerializeField] private int waveIndex;
    [SerializeField] private bool startNextWave = true;
    [SerializeField] private int[] goblinsToSpawn = new int[10];
    [SerializeField] private int[] colossusToSpawn = new int[2];
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float goblinSpawnDelay, colossusSpawnDelay;


    private GameManager GameManager;
    private UIController UIController;
    private AudioController audioController;
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

    public int WaveIndex
    {
        get => waveIndex + 1;
    } 
    #endregion

    // sets starting Position of enemies to the spawnpoint position
    void Start()
    {
        waveIndex = 0;
        startingPos = transform.position; 
        GameManager = GameObject.Find("Main Camera").GetComponent<GameManager>();
        UIController = GameObject.Find("GameCanvas").GetComponent<UIController>();
        audioController = GameObject.Find("Audio Controller").GetComponent<AudioController>();     

    }

    //counts down to the next wave of enemies and calls SpawnWave if the game is active and the next wave can start
    void Update()
    {
        if(Countdown <= 0 && GameManager.GameIsActive && startNextWave)
        {
            StartCoroutine(SpawnWave());
            startNextWave = false;
        }

        Countdown -= Time.deltaTime;
    }

    //spawns the next wave of enemies by calling SpawnGoblin and SpawnColossus a set number of times based on the waveIndex
    IEnumerator SpawnWave()
    {
        //switch for determining how many enemies to spawn
        switch(waveIndex)
        {
            //on the 5th wave spawns a boss wave
            case 4:
            {
                audioController.ChangeAudio(2);
                UIController.SetWaveText();
                for(int i = 0; i < goblinsToSpawn[waveIndex];i++)
                {
                    SpawnGoblin();
                    yield return new WaitForSeconds (goblinSpawnDelay);
                }

                audioController.ChangeAudio(3);
                for(int j = 0; j < colossusToSpawn[0]; j++)
                {
                    SpawnColossus();
                    yield return new WaitForSeconds (colossusSpawnDelay);
                }
                waveIndex++;
                Countdown = 5;
                goblinSpawnDelay = 0.8f;
                startNextWave = true;
                break;
            }

            case 5:
            case 6:
            case 7:
            case 8:
            {
                audioController.ChangeAudio(1);
                UIController.SetWaveText();
                for(int i = 0; i < goblinsToSpawn[waveIndex];i++)
                {
                    SpawnGoblin();
                    yield return new WaitForSeconds (goblinSpawnDelay);
                }
                waveIndex++;
                Countdown = 5;
                startNextWave = true;
                break;
            }

            //On the 10th wave spawns the final boss wave
            case 9:
            {
                audioController.ChangeAudio(2);
                UIController.SetWaveText();
                for(int i = 0; i < goblinsToSpawn[waveIndex];i++)
                {
                    SpawnGoblin();
                    yield return new WaitForSeconds (1.0f);
                }
                
                audioController.ChangeAudio(4);
                for(int j = 0; j < colossusToSpawn[1]; j++)
                {
                    SpawnColossus();
                    yield return new WaitForSeconds (2.0f);
                }
                waveIndex++;
                Countdown = 5;
                startNextWave = true;
                break; 
            } 

            case 10:
            {
                GameManager.EndGame("win");
                break;
            }
     
            //default case which spawns the goblins for the wave
            default:
                audioController.ChangeAudio(1);
                UIController.SetWaveText();
                for(int i = 0; i < goblinsToSpawn[waveIndex];i++)
                {
                    SpawnGoblin();
                    yield return new WaitForSeconds (goblinSpawnDelay);
                }
                waveIndex++;
                Countdown = 5;
                startNextWave = true;
                break;
        }
    }

    //Spawns a goblin
    void SpawnGoblin()
    {
        Instantiate(goblinPrefab, spawnPoint.position, transform.rotation);
    }

    //Spawns a colossus
    void SpawnColossus()
    {
        Instantiate(colossusPrefab, spawnPoint.position, transform.rotation);
    }
}    