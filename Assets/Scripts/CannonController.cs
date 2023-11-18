using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    #region [Variables]
    [SerializeField] private float range = 18f;
    [SerializeField] private float fireRate = 3f;
    [SerializeField] private Transform cannonBallSpawnPoint, targetTransform;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject cannonBall;
    [SerializeField] private GameObject cannonBlastEffect;
    [SerializeField] private AudioController audioController;
    [SerializeField] private List<GameObject> listOfEnemies = new List<GameObject>();
    [SerializeField] private Vector3 targetPosition;
    #endregion

    //invokes get target and fire methods
    void Start()
    {
        audioController = GameObject.Find("Audio Controller").GetComponent<AudioController>();

        //Gets the target twice per second after 0 second delay
        //Fires every 5 seconds after 0 second delay
        InvokeRepeating("Fire", 0, fireRate);
        InvokeRepeating("GetTarget", 0, 0.5f);
        
    }

    //Targeting V1
    void GetTarget()
    {
        //Gets all the enemies, sets the shortest distance to infinity and sets the closest enemy to null
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float shortestDistToEnemy = Mathf.Infinity;
        GameObject closestEnemy = null;

        //calculates the closest enemy
        foreach (GameObject enemy in enemies)
        {

            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy < shortestDistToEnemy) //&& enemy.GetComponent<EnemyController>().CanBeTargeted
            {
                shortestDistToEnemy = distanceToEnemy;
                closestEnemy = enemy;
            }
        }

        //sets target to closest enemy if the enemy is in range of cannon
        if(closestEnemy != null && shortestDistToEnemy <= range)
        {
            target = closestEnemy;
            targetTransform = closestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    //Spawns a cannonBall if there is a target
    void Fire()
    {
        if (targetTransform == null)
        {
            print("Missing Target Transform");
            return;
        }

        // Instantiate the cannonball at the cannonball spawn point and sets cannonball controller
        GameObject CannonBallGO = (GameObject)Instantiate(cannonBall, cannonBallSpawnPoint.position, transform.rotation);
        CannonBallController CannonBall = CannonBallGO.GetComponent<CannonBallController>();

        //Plays the cannonball explosion sound
        audioController.PlayCannonExplosionSound();

        // instantiates the cannon blast effect and destroys it after 0.5 seconds
        GameObject cannonEffectIns = (GameObject)(Instantiate(cannonBlastEffect, cannonBallSpawnPoint.position, cannonBallSpawnPoint.rotation));
        Destroy(cannonEffectIns, 0.5f);

        //sets the target of the cannonball
        if (CannonBall != null)
        {
            CannonBall.Seek(targetTransform);
        }
    }

    //Shows range when cannon is selected in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
