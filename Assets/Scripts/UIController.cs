using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    #region [Variables]
    [Header("References")]
    [SerializeField] private GameManager GameManager;
    [SerializeField] private WaveSpawner2 WaveSpawner;

    [Header("In Game UI")]
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI cannonCostText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private Button addCannonButton;

    [Header("Start Menu Objects")]
    [SerializeField] private GameObject startMenu;
    [SerializeField] private Button startButton;

    [Header("Credits Menu Objects")]
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private Button creditsMenuButton;
    [SerializeField] private Button creditsMenuBackButton;

    [Header("Misc")]
    [SerializeField] private Animator anim;
    #endregion

    //Sets game manager
    void Start()
    {
        GameManager = GameObject.Find("Main Camera").GetComponent<GameManager>();
        WaveSpawner = GameObject.Find("Spawn").GetComponent<WaveSpawner2>();

        anim = GameObject.Find("Gold Text").GetComponent<Animator>();
    }

    //Updates lives, gold, and cannon cost text
    //Probably do not need to do this every frame. could call a method every time it needs to update. but it is fine for now
    void Update()
    {
        livesText.text = "Lives: " + GameManager.Lives.ToString();
        goldText.text = "Gold: " + GameManager.Gold.ToString();
        cannonCostText.text = "Cost: " + GameManager.CannonCost.ToString();
    }


    //starts the game and disables the start menu
    public void StartGame()
    {
        GameManager.GameIsActive = true;
        startMenu.SetActive(false);
        WaveSpawner.Countdown = 5;
    }

    //Sets the text for the wave text to the wave index
    public void SetWaveText()
    {
        waveText.text = "Wave: " + WaveSpawner.WaveIndex.ToString();
    }

    public void DeactivateCannonButtonAndText()
    {
        addCannonButton.gameObject.SetActive(false);
        cannonCostText.gameObject.SetActive(false);
    }

    public void PlayNotEnoughGoldAnim()
    {
        anim.Play("GoldTextShake");
    }

    public void GoToCreditsMenu()
    {
        startMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public void ReturnToMenu()
    {
        creditsMenu.SetActive(false);
        startMenu.SetActive(true);
    }
}
