using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//NOTES:
//Probably could split the audio controller into 4 scripts instead of 2. One "Audio Controller" Which starts the audio and controlls everything
//then three helper scripts "Music Controller" "Effects Controller" and "Enemy Audio Controller" Perhaps the last two can be moved into one script depending on how big they are

public class AudioController : MonoBehaviour
{

    [Header("Music")]
    [SerializeField] private AudioSource[] music;
    [SerializeField] private int activeAudioIndex, newAudioIndex;

    [Header("Sound Effects")]
    [SerializeField] private AudioSource[] effects;

    void Start()
    {
        StartAudio();
    }

    //Starts all audio at volume 0 then sets the menu music to full volume and sets it as the active audio
    //This system is used to ensure all the audio is playing at the same time so the fading sounds seamless as all the music is in beat
    private void StartAudio()
    {
        foreach(AudioSource song in music)
        {
            song.volume = 0;
            song.Play();
        }

        music[0].volume = 1;
        activeAudioIndex = 0;
    }

    //sets the new audio and fades into new audio if it is not already playing
    public void ChangeAudio(int i)
    {
        StopAllCoroutines();
        SetNewAudio(i);

        if(activeAudioIndex != newAudioIndex)
        {
            StartCoroutine(FadeAudio());
        }
    
        return;
    }

    //Sets the new audio track (to be faded)
    public void SetNewAudio(int i)
    {
        newAudioIndex = i;
        return;
    }

    //Fades new audio into old audio by lerping the volume over the duration of the fade
    public IEnumerator FadeAudio()
    {
        float timeToFade = 2.0f;
        float timeElapsed = 0;

        while (timeElapsed < timeToFade)
        {
            music[activeAudioIndex].volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
            music[newAudioIndex].volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        ChangeActiveAudio(newAudioIndex);
        yield return null;
    }

    //Called once fade is complete to make the active audio what is currently playing
    public void ChangeActiveAudio(int i)
    {
        activeAudioIndex = i;
        return;
    }

    //Effects functions
    public void PlayCannonExplosionSound()
    {
        effects[0].Play();
    }

    public void PlayEnemyHitSound()
    {
        effects[1].Play();
    }

    public void PlayEnemyStepSoundLeft()
    {
        effects[2].Play();
    }

    public void PlayEnemyStepSoundRight()
    {
        effects[3].Play();
    }

    public void PlayEndGameAudio()
    {
        print("test");
        music[activeAudioIndex].Stop();
        music[5].volume = 1;
        music[5].Play();
    }
}

