using System;
using FMODUnity;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom Assets/SoundManagerObject")]
public class SoundManagerObject : ScriptableObject
{
    [Header("FMOD Event References - simple manager for PlayOneShot calls ONLY")]
    private static SoundManagerObject instance;

    public static SoundManagerObject Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<SoundManagerObject>("SoundManagerObject");
                if (instance == null) throw new NullReferenceException("No Sound Mnager Object is found in the resources folder ");
            }
            return instance;
        }
    }

    [SerializeField] private EventReference footstep;
    [SerializeField] private EventReference smash;
    [SerializeField] private EventReference pointGain;
    [SerializeField] private EventReference jump;
    [SerializeField] private EventReference button;
    [SerializeField] private EventReference themeMusic;
    [SerializeField] private EventReference gameover;
    [SerializeField] private EventReference transition;
    [SerializeField] private EventReference doorOpen;
    [SerializeField] private EventReference ratSpawn;
    [SerializeField] private EventReference ratDeath;
    [SerializeField] private EventReference slimeSpawn;
    [SerializeField] private EventReference barrelSpawn;
    [SerializeField] private EventReference barrelFall;
    [SerializeField] private EventReference barrelDestroy;
    [SerializeField] private EventReference startMenuMusic;
    public AudioSource musicSource;
    public AudioSource sfxSource;

    public void PlayFootstep()
    {
        RuntimeManager.PlayOneShot(footstep);
    }

    public void PlayJump()
    {
        RuntimeManager.PlayOneShot(jump);
    }

    public void PlaySmash()
    {
        RuntimeManager.PlayOneShot(smash);
    }
    public void PlayPointGain()
    {
        RuntimeManager.PlayOneShot(pointGain);
    }
    public void PlayButton()
    {
        RuntimeManager.PlayOneShot(button);
    }
    public void PlayThemeMusic()
    {
        RuntimeManager.PlayOneShot(themeMusic);
    }
    public void PlayGameover()
    {
        RuntimeManager.PlayOneShot(gameover);
    }
    public void PlayTransition()
    {
        RuntimeManager.PlayOneShot(transition);
    }
    public void PlayDoorOpen()
    {
        RuntimeManager.PlayOneShot(doorOpen);
    }
    public void PlayRatSpawn()
    {
        RuntimeManager.PlayOneShot(ratSpawn);
    }
    public void PlayRatDeath()
    {
        RuntimeManager.PlayOneShot(ratDeath);
    }
    public void PlaySlimeSpawn()
    {
        RuntimeManager.PlayOneShot(slimeSpawn);
    }
    public void PlayBarrelSpawn()
    {
        RuntimeManager.PlayOneShot(barrelSpawn);
    }
    public void PlayBarrelFall()
    {
        RuntimeManager.PlayOneShot(barrelFall);
    }
    public void PlayBarrelDestroy()
    {
        RuntimeManager.PlayOneShot(barrelDestroy);
    }
    public void PlayStartMenuMusic()
    {
        RuntimeManager.PlayOneShot(startMenuMusic);
    }  
}
