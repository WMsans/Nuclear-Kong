using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class waterSounds : MonoBehaviour
{
    public static waterSounds Instance { get; private set; }
    [SerializeField] private EventReference flowingWater;
    EventInstance flowingWaterInstance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void startSound()
    {
        if (!flowingWater.IsNull)
        {
            flowingWaterInstance = RuntimeManager.CreateInstance(flowingWater);
            flowingWaterInstance.setParameterByName("isPlaying", 0);
            flowingWaterInstance.start();
            // Set loop parameter to true
        }
        else
        {
            Debug.LogWarning("Sound EventReference is null! Please assign it in the Inspector.");
        }
    }

    public void stopSound()
    {
        if (!flowingWater.IsNull)
        {
            flowingWaterInstance.setParameterByName("isPlaying", 0);
            flowingWaterInstance.release(); // Release the instance after stopping
        }
        else
        {
            Debug.LogWarning("Sound EventReference is null! Please assign it in the Inspector.");
        }
    }
}
