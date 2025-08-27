using System;
using Michsky.LSS;
using UnityEngine;

public class LoadingNextSceneTrigger : MonoBehaviour
{
    [SerializeField] private LSS_Manager lssManager;
    [SerializeField] private SceneField nextScene;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!lssManager) lssManager = FindFirstObjectByType<LSS_Manager>();
            if(!lssManager) Debug.LogError("No lssmanager is found in the scene");
            menuMusic.Instance.stopSound();
            lssManager.LoadScene(nextScene);
        }
    }
}
