using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    #region [Variables]
    [SerializeField] private float speed;
    [SerializeField] private int health;
    [SerializeField] private int goldRewardOnDeath;
    [SerializeField] private Vector3 startingPos;
    [SerializeField] private EnemyController enemy;
    [SerializeField] private GameManager GameManager;
    [SerializeField] private GameObject enemyIsHitEffect;

    [SerializeField] private bool enemySoundOnHit;
    [SerializeField] private AudioController audioController;
    [SerializeField] private float stopPositionX;

    [SerializeField] private bool enemyIsAttacking = false;

    [SerializeField] private Animator animator;

    public Vector3 pos;
    #endregion

    #region [Variable Properties]
    public int Health
    {
        get => health;
        set => health = value;
    }

    public bool EnemyIsAttacking
    {
        get => enemyIsAttacking;
        set => enemyIsAttacking = value;
    }
    #endregion

    // sets starting position, game manager, and enemy
    void Start() 
    {  
        audioController = GameObject.Find("Audio Controller").GetComponent<AudioController>();
        GameManager = GameObject.Find("Main Camera").GetComponent<GameManager>();

        startingPos = transform.position;
        enemy = this.gameObject.GetComponent<EnemyController>();

        animator = this.gameObject.GetComponent<Animator>();
    }

    //Moves enemy towards castle
    //TODO need to stop the enemy just short of the castle and do one damage per second (invoke? enemyAttack method every second?)
    void Update()
    {
        pos = transform.position;

        if(pos.x < stopPositionX)
        {
            transform.position += (Vector3.right * speed * Time.deltaTime);
        }
        else if(!enemyIsAttacking)
        {
            enemyIsAttacking = true;
            PlayAnimation();
        }
    }

    //damages the enemy and checks to see if enemy has any health left
    public void DamageEnemy(int damage)
    {
        enemy.Health -= damage;

        //Instantiates enemy is hit effect and destroys it after 0.5 seconds
        GameObject enemyIsHitEffectIns = (GameObject)(Instantiate(enemyIsHitEffect, transform.position, transform.rotation));
        Destroy (enemyIsHitEffectIns, 0.5f);

        if(enemy.Health < 1)
        {
            KillEnemy();
        }
    }

    //rewards player for killing enemy and destroys the enemy
    public void KillEnemy()
    {
        GameManager.Gold += goldRewardOnDeath;

        if(enemySoundOnHit)
        {
            audioController.PlayEnemyHitSound();
        }
        //else EnemyAudioController.StopEnemyStepSound();
   
        Destroy(gameObject);
    }

    public void PlayAnimation()
    {
        animator.SetBool("enemyIsAttacking", true);
    }
}
