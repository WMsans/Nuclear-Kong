using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class ConveyerSounds : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static ConveyerSounds Instance { get; private set; }
    [SerializeField] private EventReference conveyerBeltSound;
    EventInstance conveyerBeltInstance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // Keep this GameObject alive when loading new scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If another instance already exists, destroy this duplicate
            Destroy(gameObject);
        }
    }
    public void startSound()
    {
        if (!conveyerBeltSound.IsNull)
        {
            conveyerBeltInstance = RuntimeManager.CreateInstance(conveyerBeltSound);
            conveyerBeltInstance.setParameterByName("conveyerMoving", 1);
            conveyerBeltInstance.start();
            // Set loop parameter to true
        }
        else
        {
            Debug.LogWarning("Conveyer Belt Sound EventReference is null! Please assign it in the Inspector.");
        }
    }
    public void stopSound()
    {
        if (!conveyerBeltSound.IsNull)
        {
            conveyerBeltInstance.setParameterByName("conveyerMoving", 0);
            conveyerBeltInstance.release(); // Release the instance after stopping
        }
        else
        {
            Debug.LogWarning("Conveyer Belt Sound EventReference is null! Please assign it in the Inspector.");
        }
    }  
}
