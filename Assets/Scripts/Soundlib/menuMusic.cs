using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class menuMusic : MonoBehaviour
{
    public static menuMusic Instance { get; private set; }
    [SerializeField] private EventReference menuMusicSound;
    EventInstance menuMusicInstance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void startSound()
    {
        if (!menuMusicSound.IsNull)
        {
            menuMusicInstance = RuntimeManager.CreateInstance(menuMusicSound);
            menuMusicInstance.setParameterByName("isPlaying", 0);
            menuMusicInstance.start();
            // Set loop parameter to true
        }
        else
        {
            Debug.LogWarning("Sound EventReference is null! Please assign it in the Inspector.");
        }
    }

    public void stopSound()
    {
        if (!menuMusicSound.IsNull)
        {
            menuMusicInstance.setParameterByName("isPlaying", 1);
            menuMusicInstance.release(); // Release the instance after stopping
        }
        else
        {
            Debug.LogWarning("Sound EventReference is null! Please assign it in the Inspector.");
        }
    }
}
