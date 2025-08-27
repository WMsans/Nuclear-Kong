using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class FullScreenFade : MonoSingleton<FullScreenFade>
{
    private static readonly int FadeAmount = Shader.PropertyToID("_FadeAmount");
    [SerializeField] private float fadeInRate;
    [SerializeField] private float fadeOutRate;
    private Material _mat;
    public UnityEvent onFadeInComplete;
    public UnityEvent onFadeOutComplete;
    protected override void Awake()
    {
        base.Awake();
        _mat = GetComponent<SpriteRenderer>().material;
    }

    public void FadeIn(float duration = -1f)
    {
        DOTween.To(() => _mat.GetFloat(FadeAmount), x => _mat.SetFloat(FadeAmount, x), 0, duration < 0 ? fadeInRate : duration).OnComplete(() =>
        {
            onFadeInComplete.Invoke();
        }).SetUpdate(true);
    }

    public void FadeOut(float duration = -1f)
    {
        DOTween.To(() => _mat.GetFloat(FadeAmount), x => _mat.SetFloat(FadeAmount, x), 1, duration < 0 ? fadeOutRate : duration).OnComplete(() =>
        {
            onFadeOutComplete.Invoke();
        }).SetUpdate(true);
    }
}
