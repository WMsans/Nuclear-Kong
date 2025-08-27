//Remember to import FMODUnity and FMOD.Studio!
using FMODUnity;
using FMOD.Studio;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    [Header("FMOD Event References - simple manager for PlayOneShot calls ONLY")]
    public static SoundManager Instance { get; private set; }
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    [SerializeField] private EventReference barrelDestroy;
    [SerializeField] private EventReference barrelExplode;
    [SerializeField] private EventReference barrelFall;
    [SerializeField] private EventReference barrelSpawn;
    [SerializeField] private EventReference button;
    // [SerializeField] private EventReference conveyer; covered in other script
    [SerializeField] private EventReference doorDestroy;
    [SerializeField] private EventReference doorOpen;
    [SerializeField] private EventReference doorSmash;
    // [SerializeField] private EventReference flowingWater; covered in other script
    [SerializeField] private EventReference gameEnd;
    // [SerializeField] private EventReference gameMenu; covered in other script
    [SerializeField] private EventReference gameTransition;
    [SerializeField] private EventReference obtainWeapon;
    [SerializeField] private EventReference playerJump;
    [SerializeField] private EventReference playerLand;
    [SerializeField] private EventReference playerPoints;
    [SerializeField] private EventReference playerSmash; // note this will need an input parameter 0 for hammer or 1 for katana
    [SerializeField] private EventReference playerWalk;
    [SerializeField] private EventReference ratDeath;
    [SerializeField] private EventReference ratSpawn;
    [SerializeField] private EventReference slimeDeath;
    [SerializeField] private EventReference slimeSpawn;
    [SerializeField] private EventReference bossMusic;
    [SerializeField] private EventReference wallDestroy;
    [SerializeField] private EventReference menuMusic;
    public AudioSource musicSource;
    public AudioSource sfxSource;
    private EventInstance bossMusicInstance;
    private EventInstance menuMusicInstance;

    public void PlayBarrelDestroy() { RuntimeManager.PlayOneShot(barrelDestroy); }
    public void PlayBarrelExplode() { RuntimeManager.PlayOneShot(barrelExplode); }
    public void PlayBarrelFall() { RuntimeManager.PlayOneShot(barrelFall); }
    public void PlayBarrelSpawn() { RuntimeManager.PlayOneShot(barrelSpawn); }
    public void PlayButton(int x)
    {
        EventInstance buttonInstance = RuntimeManager.CreateInstance(button);
        buttonInstance.setParameterByName("buttonNum", x);
        buttonInstance.start();
        buttonInstance.release();
    }
    public void PlayDoorDestroy() { RuntimeManager.PlayOneShot(doorDestroy); }
    public void PlayDoorOpen() { RuntimeManager.PlayOneShot(doorOpen); }
    public void PlayDoorSmash() { RuntimeManager.PlayOneShot(doorSmash); }
    public void PlayGameEnd() { RuntimeManager.PlayOneShot(gameEnd); }
    public void PlayGameTransition() { RuntimeManager.PlayOneShot(gameTransition); }
    public void PlayObtainWeapon() { RuntimeManager.PlayOneShot(obtainWeapon); }
    public void PlayPlayerJump() { RuntimeManager.PlayOneShot(playerJump); }
    public void PlayPlayerLand() { RuntimeManager.PlayOneShot(playerLand); }
    public void PlayPlayerPoints() { RuntimeManager.PlayOneShot(playerPoints); }
    public void PlayPlayerSmash(int weaponType)
    {
        EventInstance smashInstance = RuntimeManager.CreateInstance(playerSmash);
        smashInstance.setParameterByName("smashType", weaponType);
        smashInstance.start();
        smashInstance.release();
    }
    public void PlayPlayerWalk() { RuntimeManager.PlayOneShot(playerWalk); }
    public void PlayRatDeath() { RuntimeManager.PlayOneShot(ratDeath); }
    public void PlayRatSpawn() { RuntimeManager.PlayOneShot(ratSpawn); }
    public void PlaySlimeDeath() { RuntimeManager.PlayOneShot(slimeDeath); }
    public void PlaySlimeSpawn() { RuntimeManager.PlayOneShot(slimeSpawn); }
    public void startBossMusic()
    {
        bossMusicInstance = RuntimeManager.CreateInstance(bossMusic);
        bossMusicInstance.start();
    }
    public void updateBossMusic(int bossAlive)
    {
        bossMusicInstance.setParameterByName("bossAlive", bossAlive);
        bossMusicInstance.release();
    }
    public void PlayWallDestroy() { RuntimeManager.PlayOneShot(wallDestroy); }
    public void startMenuMusic()
    {
        menuMusicInstance = RuntimeManager.CreateInstance(bossMusic);
        menuMusicInstance.start();
    }
    public void stopMenuMusic()
    {
        menuMusicInstance.setParameterByName("isPlaying", 1);
        menuMusicInstance.release();
    }

}
