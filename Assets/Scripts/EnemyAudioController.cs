using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioController : MonoBehaviour
{

    [SerializeField] private AudioController audioController;

    // Start is called before the first frame update
    void Start()
    {
        audioController = GameObject.Find("Audio Controller").GetComponent<AudioController>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Ground")
        {
            if (this.gameObject.tag == "left")
            {
                audioController.PlayEnemyStepSoundLeft();
            }
            else if (this.gameObject.tag == "right")
            {
                audioController.PlayEnemyStepSoundRight();
            }
        }

        else if (this.gameObject.tag == "weapon")
        {
            audioController.PlayEnemyWeaponHitSound();
        }
    }
}
