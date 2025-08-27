using UnityEngine;
using System.Collections.Generic;

public class MenuManager : MonoSingleton<MenuManager>
{
    [Header("Pause Menu Settings")]
    [Tooltip("The UI elements to activate when the game is paused.")]
    [SerializeField] private List<GameObject> pauseMenuActiveObjects;

    [Tooltip("The GameObjects to deactivate when the game is paused.")]
    [SerializeField] private List<GameObject> objectsToDeactivateOnPause;

    public bool IsPaused { get; private set; }

    private void Update()
    {
        if (InputSystemManager.Instance.CurrentFrameInput.MenuDown)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;

        if (IsPaused)
        {
            Time.timeScale = 0f;
            SetObjectsActive(pauseMenuActiveObjects, true);
            SetObjectsActive(objectsToDeactivateOnPause, false);
        }
        else
        {
            Time.timeScale = 1f;
            SetObjectsActive(pauseMenuActiveObjects, false);
            SetObjectsActive(objectsToDeactivateOnPause, true);
        }
    }

    private void SetObjectsActive(List<GameObject> objects, bool isActive)
    {
        foreach (var obj in objects)
        {
            if (obj != null)
            {
                obj.SetActive(isActive);
            }
        }
    }
}