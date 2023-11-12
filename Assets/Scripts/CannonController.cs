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
    [SerializeField] private int enemyIndexTest = 0;
    #endregion

    //invokes get target and fire methods
    void Start()
    {
        audioController = GameObject.Find("Audio Controller").GetComponent<AudioController>();

        //Gets the target twice per second after 0 second delay
        //Fires every 5 seconds after 0 second delay
        InvokeRepeating("Fire", 0, fireRate);
        
    }

    void Update()
    {
        // foreach (GameObject enemy in listOfEnemies)
        // {
        //     if (enemy == null)
        //     {
        //         RemoveTarget(enemy);
        //     }
        // }

        listOfEnemies.RemoveAll(item => item == null);

        if(listOfEnemies.Count > enemyIndexTest)
        {
            GetTargetEnemy(enemyIndexTest);
        }
        else
        {
            enemyIndexTest = 0;
            GetTargetEnemy(enemyIndexTest);
        }
        
    }
    
    //Targeting V3
    void AddTarget(GameObject _target)
    {
        listOfEnemies.Add(_target);
    }

    void RemoveTarget(GameObject _target)
    {
        listOfEnemies.Remove(_target);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            AddTarget(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            RemoveTarget(other.gameObject);
        }
    }

    void GetTargetEnemy(int i)
    {
        if(listOfEnemies.Count == 0)
        {
            return;
        }

        target = listOfEnemies[i];

        if(target == null)
        {
            return;
        }

        if(target.GetComponent<EnemyController>().CanBeTargeted)
        {
            target.GetComponent<EnemyController>().EnemyIsTargeted();
            targetTransform = target.transform;
        }
        else
        {
            enemyIndexTest++; 
        }


    }

    //Targeting V2
    // void TargetEnemy()
    // {
    //     ScanForEnemies();
    //     GetTargetEnemy(enemyIndexTest);

    // }

    // void ScanForEnemies()
    // {
    //     listOfEnemies.Clear();

    //     GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

    //     foreach (GameObject enemy in enemies)
    //     {
    //         listOfEnemies.Add(enemy);
    //     }
    // }

    // void GetTargetEnemy(int i)
    // {
    //     int len = listOfEnemies.Count;

    //     if (len <= 0)
    //     {
    //         return;
    //     }

    //     target = listOfEnemies[i];

    //     float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

    //     if (target != null && distanceToTarget <= range)
    //     {
    //         if (target.GetComponent<EnemyController>().CanBeTargeted)
    //         {
    //             target.GetComponent<EnemyController>().EnemyIsTargeted();
    //             targetTransform = target.transform;
    //         }
    //         else
    //         {
    //             enemyIndexTest++;
    //             if (enemyIndexTest < listOfEnemies.Count)
    //             {
    //                 GetTargetEnemy(enemyIndexTest);
    //             }
    //         }
    //     }
    // }

    //Targeting V1
    // void GetTarget()
    // {
    //     //Gets all the enemies, sets the shortest distance to infinity and sets the closest enemy to null
    //     GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
    //     float shortestDistToEnemy = Mathf.Infinity;
    //     GameObject closestEnemy = null;

    //     //calculates the closest enemy
    //     foreach (GameObject enemy in enemies)
    //     {

    //         float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
    //         if(distanceToEnemy < shortestDistToEnemy && enemy.GetComponent<EnemyController>().CanBeTargeted) //&& enemy.GetComponent<EnemyController>().CanBeTargeted
    //         {
    //             shortestDistToEnemy = distanceToEnemy;
    //             closestEnemy = enemy;

    //         }
    //     }

    //     //sets target to closest enemy if the enemy is in range of cannon
    //     if(closestEnemy != null && shortestDistToEnemy <= range && closestEnemy.GetComponent<EnemyController>().CanBeTargeted)
    //     {
    //         target = closestEnemy.transform;
    //         closestEnemy.GetComponent<EnemyController>().EnemyIsTargeted();
    //     }
    //     else
    //     {
    //         target = null;
    //     }
    // }

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
