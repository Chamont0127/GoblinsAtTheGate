using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region[Variables]
    [SerializeField] private int gold;
    [SerializeField] private int startingGold = 200;
    [SerializeField] private int lives;
    [SerializeField] private int startingLives = 100;
    [SerializeField] private int cannonCost;
    [SerializeField] private int cannonStartCost = 100;
    [SerializeField] private bool gameIsActive;
    [SerializeField] private int numActiveEnemies;
    [SerializeField] private bool gameCanEnd;

    private AudioController audioController;
    private UIController UIController;
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
            if (value > 0)
                lives = value;
            else
            {
                lives = 0;
                if (gameIsActive)
                    EndGame("lose");
            }
        }
    }

    //cannon cost doubles every time it is set
    public int CannonCost
    {
        get => cannonCost;

        set
        {
            if (value < 201)
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

    public int NumActiveEnemies { get => numActiveEnemies; set => numActiveEnemies = value; }
    #endregion

    // Sets gold, lives, and cannonCost to starting values
    void Start()
    {
        gameIsActive = false;
        gameCanEnd = false;
        gold = startingGold;
        lives = startingLives;
        cannonCost = cannonStartCost;

        audioController = GameObject.Find("Audio Controller").GetComponent<AudioController>();
        UIController = GameObject.Find("GameCanvas").GetComponent<UIController>();
    }

    //Checks if lose condition is met
    void Update()
    {
         if (numActiveEnemies <= 0 && gameCanEnd)
            { //TODO: Need to change this to display win game over UI
                UIController.ShowGameOverMenu();
            }
    }

    public void EndGame(string endCondition)
    {
        gameIsActive = false;

        

        if (endCondition == "lose")
        {
            UIController.ShowGameOverMenu();
            audioController.PlayEndGameAudio();
            return;
        }

        else if (endCondition == "win")
        {
            gameCanEnd = true;
            audioController.PlayEndGameAudio();
            return;
        }

    }

    public void ReloadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void PlayButtonSoundEffect()
    {
        audioController.PlayButtonClickSound();
    }
}
