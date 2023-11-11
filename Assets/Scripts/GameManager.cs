using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region[Variables]
    [SerializeField] private int gold;
    [SerializeField] private int startingGold = 200;
    [SerializeField] private int lives;
    [SerializeField] private int startingLives = 10;
    [SerializeField] private int cannonCost;
    [SerializeField] private int cannonStartCost = 100;
    [SerializeField] private bool gameIsActive;

    private AudioController audioController;

    //private WaveSpawner WaveSpawner;
    #endregion

    #region [VariableProperties]
    public int Gold
        {
            get => gold;
            set => gold = value;
        }

    //setter logic to make sure lives does not go below 0
    public int Lives
        {
            get => lives;
            set
                {
                    if(value >= 0)
                        lives = value;
                    else
                    {
                        lives = 0;
                        if(gameIsActive)
                            EndGame("lose");
                    }
                }
        }
    
    //cannon cost doubles every time it is set
    //BUG should fix later so it does not go past 400
    public int CannonCost
        {
            get => cannonCost;

            set
            {
                if(value < 201)
                {
                    cannonCost = value * 2;
                }
            }
        }

    public bool GameIsActive
    {
        get => gameIsActive;
        set => gameIsActive = value;
    }
    #endregion

    // Sets gold, lives, and cannonCost to starting values
    void Start()
    {
        gameIsActive = false;
        gold = startingGold;
        lives = startingLives;
        cannonCost = cannonStartCost;

        audioController = GameObject.Find("Audio Controller").GetComponent<AudioController>();
    }

    //Checks if lose condition is met
    void Update()
    {
        //TODO only do this is the game is not over otherwise it will spam the shit out of this call
        if (lives == 0)
        {
            //EndGame();
        }
    }

    public void EndGame(string endCondition)
        {
            gameIsActive = false;

            audioController.PlayEndGameAudio();

            if(endCondition == "lose")
            {
                //Display lose game over UI
                return;
            }

            else if(endCondition == "win")
            {
                //Display win game over UI
                return;
            }

        }
}
