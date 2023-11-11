using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallController : MonoBehaviour
{
    #region [Variables]
    [SerializeField] private float speed = 15f;
    [SerializeField] private int damage = 1;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject CannonBallEffect;
    #endregion

    //Moves the cannon ball and destroys it if there is no target
    //NOTES: Since this happens every frame it makes the cannon ball curved. One option would be to call a GetTarget function which sets the target then the cannon fires at that
    void Update()
    {
        //if there is no target destroy the cannonBall
        if(target != null)
        {
            //gets direction to move in
            Vector3 dir = target.position - transform.position;
            float distanceThisFrame = speed * Time.deltaTime; 

            //moves the cannonball to the target
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
            transform.LookAt(target);
        }
        else
            Destroy(gameObject);

        
    }

    //sets target (used from CannonController class)
    public void Seek(Transform t)
    {
        target = t;
    }

    // damages enemy based on damage and destroys the cannon ball
    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Enemy") 
        {
            //Instantiate the cannon ball impact effect and destroys it after 0.5 seconds
            GameObject cannonBallEffectIns = (GameObject)(Instantiate(CannonBallEffect, transform.position, transform.rotation));
            Destroy (cannonBallEffectIns, 0.5f);

            //damages the enemy and destroys the cannon ball
            coll.gameObject.GetComponent<EnemyController>().DamageEnemy(damage);
            Destroy(gameObject);
        }
    }
}
