using System;
using DG.Tweening;
using Michsky.LSS;
using UnityEngine;
using VInspector;

public class MainMenuButtonPanel : MonoBehaviour
{
    [SerializeField] private Vector2 startPos;

    [Header("Scene Transiton")] 
    [SerializeField] private SceneField startScene;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = transform as RectTransform;
    }

    private void Start()
    {
        SlideIn();
        menuMusic.Instance.startSound();
    }
    private void SlideIn()
    {
        var oriPos = _rectTransform.position;
        _rectTransform.position = startPos;
        _rectTransform.DOMove(oriPos, 1f).SetEase(Ease.OutQuad);
    }

    public void OnStartGame()
    {
        menuMusic.Instance.stopSound();
        var loadmangaer = FindFirstObjectByType<LSS_Manager>();
        loadmangaer.LoadScene(startScene);
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }
}
