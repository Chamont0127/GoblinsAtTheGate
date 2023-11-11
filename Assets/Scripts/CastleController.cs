using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastleController : MonoBehaviour
{
    #region [Variables]
    [SerializeField] private int currentGold;
    [SerializeField] private int currentCannonPrice;
    [SerializeField] private GameObject enemyGO;
    [SerializeField] private GameManager GameManager;
    [SerializeField] private UIController UIController;

    [Header("Cannon Elements")]
    [SerializeField] private int cannonIndex;
    [SerializeField] private GameObject cannonPrefab;
    [SerializeField] private Vector3 cannonSpawnPoint;
    //[SerializeField] private Button addCannonButton;
    

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints = new Transform[3];
    #endregion

    //sets cannon index and game manager
    void Start()     
    {
        cannonIndex = 0;
        GameManager = GameObject.Find("Main Camera").GetComponent<GameManager>();     
        UIController = GameObject.Find("GameCanvas").GetComponent<UIController>();   
    }

    //Sets the spawnpoint for the cannon at the next index
    public void SetCannonSpawn()
    {
        //gets the current gold and current cannon price from GameManager
        currentGold = GameManager.GetComponent<GameManager>().Gold;
        currentCannonPrice = GameManager.GetComponent<GameManager>().CannonCost;

        // exits if index is out of bounds, else spawns cannon at the position of the spawnpoint of the cannon index
        if(cannonIndex > 2)
        {
            return;
        }
        else
        {
            cannonSpawnPoint = spawnPoints[cannonIndex].position;
            BuyCannon();
        }
    }

    //checks if player has enough money to buy cannon and if the add cannon button should be destroyed
    void BuyCannon()
    {
        //adds a cannon at cannon spawn if player can afford cannon
        if(currentCannonPrice <= currentGold)
        {
            AddCannon(cannonSpawnPoint);
            GameManager.GetComponent<GameManager>().Gold -= currentCannonPrice;
            cannonIndex++;
            GameManager.GetComponent<GameManager>().CannonCost = currentCannonPrice;

            //checks if add cannon button should be destroyed
            if(cannonIndex == 3)
            {
                UIController.DeactivateCannonButtonAndText();
            }
        }
        else    
            UIController.PlayNotEnoughGoldAnim();
    }
   
    //Spawns the cannon
    public void AddCannon(Vector3 pos)
    {
        Instantiate(cannonPrefab, pos, transform.rotation);
    }

    //if an enemy enters the trigger decrease lives by one and destroy the enemy
    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {   
            GameManager.Lives--;
            Destroy(coll.gameObject);
        }
    }
}
