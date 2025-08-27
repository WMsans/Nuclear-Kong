using UnityEngine;

public class ThemePlayer : MonoBehaviour
{
    public static ThemePlayer Instance { get; private set; }
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

    public void PlayTheme()
    {

    }
    public void StopTheme()
    {
        
    }
}
